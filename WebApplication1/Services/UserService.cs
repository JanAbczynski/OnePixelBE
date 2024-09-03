using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Services
{
    public class UserService
    {
        readonly AppDbContext _context;
        private IConfiguration _config;
        private CommonService _commonService;
        private IMapper _mapper;

        public UserService(AppDbContext context, IConfiguration config, CommonService commonService, IMapper mapper)
        {
            _context = context;
            _config = config;
            _commonService = commonService;
            _mapper = mapper;
        }


        internal CustomResponse TryLogin(UserViewModel user)
        {
            CustomResponse result = AuthenticateUser(user);


            if (!result.Status)
            {
                return result;
            }
            user.Id = (Guid)result.Data;
            string tokenStr = GenerateJSOWebToken(user);
            result = new CustomResponse { Data = tokenStr, Status = true, Message = "Token" };

            return result;
        }

        internal CustomResponse TryToRegister(User user)
        {
            user.Id = Guid.NewGuid();
            user.Type = UserType.person;
            user.Role = UserRole.admin;

            User existingUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);
            if (existingUser != null)
            {
                return new CustomResponse() { Status = false, Message = $"Email {user.Email} jest już zarejestrowany" };
            }
            var saltAndPass = GenerateSaltAndPass(user.Password);
            Code code = GenerateCode(CodeType.emailVeryfication, user);

            user.Password = saltAndPass.pass;
            user.Salt = saltAndPass.salt;

            _context.Add(code);
            _context.Add(user);
            _context.SaveChanges();

            MailOperator(user, MailType.varyfication, code.RawCode);

            return new CustomResponse() { Status = true, Message = "Zarejestrowano użytkownika" };
        }

        private (string salt, string pass) GenerateSaltAndPass(string password)
        {
            var saltAsByte = GetSalt();
            var saltAsString = Encoding.UTF8.GetString(saltAsByte, 0, saltAsByte.Length);
            var hashedPassword = HashPassword(saltAsByte, password);

            return (salt: saltAsString, pass: hashedPassword);
        }

        private Code GenerateCode(CodeType codeType, User user, string additionalInfo = null)
        {

            Code code = new Code();
            code.RawCode = Guid.NewGuid().ToString();
            code.CreationDate = DateTime.Now;

            switch (codeType)
            {
                case CodeType.other:
                    break;
                case CodeType.changePassword:
                    code.ExpireDate = code.CreationDate.AddHours(1);
                    code.ConnectedUserId = user.Id;
                    code.codeType = codeType;
                    code.IsActive = true;
                    code.WasUsed = false;
                    break;
                case CodeType.emailVeryfication:
                    code.ExpireDate = code.CreationDate.AddDays(7);
                    code.ConnectedUserId = user.Id;
                    code.codeType = codeType;
                    code.IsActive = true;
                    code.WasUsed = false;
                    break;
                case CodeType.increaseCurrency:
                    break;
                case CodeType.changeEmail:
                    code.ExpireDate = code.CreationDate.AddDays(1);
                    code.ConnectedUserId = user.Id;
                    code.codeType = codeType;
                    code.IsActive = true;
                    code.WasUsed = false;
                    code.AdditionalInfo = additionalInfo;
                    break;
            }
            return code;
        }

        private bool MailOperator(User userReciever, MailType mailType, string code = null)
        {
            EmailSender email = new EmailSender();
            string mailBody = email.BodyBuilder(code, mailType, userReciever);
            string mailSubject = email.CreateSubject(mailType);
            email.PrepareEmail(userReciever.Email, mailBody, mailSubject);
            return true;
        }

        private byte[] GetSalt()
        {
            var salt = GenerateSalt();
            return salt;
        }

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[5];
                rng.GetBytes(randomNumber);
                var temp_string = Encoding.UTF8.GetString(randomNumber, 0, randomNumber.Length);
                var randomNumberUTF8 = Encoding.UTF8.GetBytes(temp_string);
                return randomNumberUTF8;
            }
        }

        private CustomResponse AuthenticateUser(UserViewModel user)
        {
            User userFromDB = _context.Users.FirstOrDefault(x => x.Email == user.Email);
            CustomResponse response = new CustomResponse() { Message = "Nieznany błąd", Status = false };
            if (userFromDB == null)
            {
                response.Message = "Podany email nie jest zarejsetrowany";
                return response;
            }
            var saltAsString = userFromDB.Salt;
            byte[] saltAsByte = Encoding.UTF8.GetBytes(saltAsString);
            var hashedPassword = HashPassword(saltAsByte, user.Password);

            if (hashedPassword == userFromDB.Password)
            {
                if (!userFromDB.IsConfirmed)
                {
                    response.Message = "Email nie został jeszcze potwierdzony";
                    return response;
                }
                response.Message = "Autoryzacja poprawna";
                response.Status = true;
                response.Data = userFromDB.Id;
                return response;
            }
            else
            {
                response.Message = "Błędne hasło";
                return response;
            }
        }

        internal CustomResponse TryPassReset(UserViewModel userVM)
        {
            User userModel = _context.Users.FirstOrDefault(x => x.Email == userVM.Email);
            if (userModel == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono użytkownika" };
            }
            Code code = GenerateCode(CodeType.changePassword, userModel);
            _context.Add(code);
            _context.SaveChanges();
            MailOperator(userModel, MailType.changePassword, code.RawCode);

            return new CustomResponse() { Status = true, Message = "Wysłano email z potwierdzeniem." };
        }

        internal CustomResponse TryPassResetStep2(UserViewModel userVM, Code codeFromDB)
        {
            var saltAndPass = GenerateSaltAndPass(userVM.Password);
            User user = _context.Users.FirstOrDefault(x => x.Id == codeFromDB.ConnectedUserId);

            user.Password = saltAndPass.pass;
            user.Salt = saltAndPass.salt;
            _context.SaveChanges();

            return new CustomResponse() { Status = true, Message = "Hasło zostało zmienione" };
        }

        internal CustomResponse TryChangePermissionsData(UserViewModel userVM)
        {
            var x = _context.Users.Include(p => p.Permissions).FirstOrDefault(x => x.Id == userVM.Id);


            foreach(UserPermissionViewModel permission in userVM.Permissions)
            {
                permission.UserId = userVM.Id;
            }

            AddPermission(userVM.Permissions);

            //foreach (UserPermissionViewModel onePermission in userVM.Permissions)
            //{
            //    if (onePermission.Id == Guid.Empty)
            //    {
            //        onePermission.Id = Guid.NewGuid();
            //    }
            //    if (onePermission.RawPermissionId == Guid.Empty)
            //    {
            //        onePermission.RawPermissionId = onePermission.RawPermission.Id;
            //        onePermission.UserId = userVM.Id;
            //    }

            //}



            return new CustomResponse() { Status = true };
        }

        private void AddPermission(List<UserPermissionViewModel> permissionsVM) {
            
            foreach(UserPermissionViewModel permissionVM in permissionsVM)
            {
                UserPermission permission = _mapper.Map<UserPermission>(permissionVM);
                permission.RawPermissionID = permissionVM.RawPermission.Id;
                _context.Add(permission);
            }
        }

        internal CustomResponse TryChangeEmail(UserViewModel userVM)
        {
            User userFromInput = _mapper.Map<User>(userVM);
            User userFromDB = _context.Users.FirstOrDefault(x => x.Email == userVM.Email);
            Code code=GenerateCode(CodeType.changeEmail, userFromDB, userVM.SecondEmail);

            _context.Add(code);
            _context.SaveChanges();

            userFromInput.Email = userVM.SecondEmail;
            MailOperator(userFromInput, MailType.changeEmail, code.RawCode);

            CustomResponse response = new CustomResponse() {Status = true, Message = "Rozpoczęto zmiana adresu email" };
            return response;
        }

        internal CustomResponse TryChangePassword(UserViewModel user)
        {
            User userFormDB = _context.Users.FirstOrDefault(x => x.Email == user.Email);
            if(userFormDB == null)
            {
                return new CustomResponse() { Status = false, Message = "Hasło nie zostało zmienione" };
            }
           CustomResponse response = AuthenticateUser(user);
            if (response.Status)
            {
                var saltAndPass = GenerateSaltAndPass(user.NewPassword);
                userFormDB.Password = saltAndPass.pass;
                userFormDB.Salt = saltAndPass.salt;
                _context.SaveChanges();
                return new CustomResponse() { Status = true, Message = "Hasło zostało zmienione" };
            }

            return response;
        }

        internal List<string> TryGetAllowMenu(User user)
        {
            if (user == null)
            {
                user = new User() { };
                user.Role = UserRole.mrNobody;
            }
            var menuList = _context.MenuAccessDatas.Where(x => x.Role == user.Role).Select(xx => xx.SideMenuName).ToList();

            return menuList;
        }

        internal List<MenuOutput> TryGetAllowMenu2(UserViewModel user)
        {
            if (user == null)
            {
                user = new UserViewModel() { };
                user.Role = UserRole.mrNobody;
            }
            var menuList = _context.MenuParts.Where(x => x.UserRole == user.Role).OrderBy(x => x.MainMenu).ToList();

            List<MenuOutput> menuOutput = new List<MenuOutput>() { };
            List<string> tempMenu = new List<string>() { };
            foreach(var menu in menuList)
            {
                string MainMenuString = menu.MainMenu;

                if (!tempMenu.Contains(MainMenuString)){
                    tempMenu.Add(MainMenuString);
                }
            }

            foreach(string oneMainMenu in tempMenu)
            {
                MenuOutput newMainMenu = new MenuOutput() { };
                newMainMenu.MainMenu = oneMainMenu;
                newMainMenu.SubMenu = new List<SubMenuOutput>() { };
                menuOutput.Add(newMainMenu);
            }

            foreach(var menu in menuList)
            {
                SubMenuOutput subMenu = new SubMenuOutput() { };
                subMenu.SubMenu = menu.SubMenu;
                subMenu.Link = menu.RouterLink;

                foreach(var existingMainMenu in menuOutput)
                {
                    if(existingMainMenu.MainMenu == menu.MainMenu)
                    {
                        existingMainMenu.SubMenu.Add(subMenu);
                    }
                }
            }

            return menuOutput;
        }

        internal List<AvaliblePermissionDetail> TryGetPermissionDetail(Guid permissionId)
        {
            List<AvaliblePermissionDetail> permissionDetails = _context.AvaliblePermissionDetail.Where(x => x.PermissionId == permissionId).ToList();

            return permissionDetails;
        }

        internal List<AvaliblePermissions> TryGetPermissions()
        {
            List<AvaliblePermissions> permissions = _context.AvaliblePermissions.OrderBy(x => x.Name).ToList();


            return permissions;
        }

        private UserViewModel GetUserDataByToken(string token)
        {
            UserViewModel userViewModel = new UserViewModel() { };
            if(token == null)
            {
                return userViewModel;
            }
            var jwt = token.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDecoded = tokenHandler.ReadJwtToken(jwt);
            //userViewModel.UserLogin = tokenDecoded.Subject;
            userViewModel.Email = tokenDecoded.Claims.FirstOrDefault(x => x.Type == "email").Value;
            userViewModel.Role = (UserRole)Enum.Parse(typeof(UserRole), tokenDecoded.Claims.FirstOrDefault(x => x.Type == "role").Value);
            userViewModel.Type = (UserType)Enum.Parse(typeof(UserType), tokenDecoded.Claims.FirstOrDefault(x => x.Type == "userType").Value);
            userViewModel.Id = Guid.Parse(tokenDecoded.Claims.FirstOrDefault(x => x.Type == "userId").Value);

            return userViewModel;
        }

        internal User TryChangePersonalData(User user)
        {
            User existingUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.BirthDate = user.BirthDate;
            existingUser.Gender = user.Gender;

            _context.SaveChanges();


            return RetypeUser(existingUser);
            //return ClearSensitiveData(existingUser);
        }

        internal User TryChangeAddressData(User user)
        {
            User existingUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            existingUser.Country = user.Country;
            existingUser.ZipCode = user.ZipCode;
            existingUser.City = user.City;
            existingUser.Street = user.Street;
            existingUser.BuildingNumber = user.BuildingNumber;
            existingUser.LocalNumber = user.LocalNumber;

            _context.SaveChanges();

            //return ClearSensitiveData(existingUser);
            return RetypeUser(existingUser);
        }

        internal List<EnumViewModel> TryGetGender()
        {
            List<EnumViewModel> genderVMList = _commonService.EnumToList<Gender>();

            return genderVMList;
        }


        internal CustomResponse TryGetUserData(string bearerToken)
        {
            CustomResponse response = new CustomResponse() { };
            User user = new User() { };
            User tempUser = new User() { };
            if (bearerToken != null && bearerToken != "")
            {
                string token = bearerToken.Replace("Bearer ", "");
                UserViewModel userVM = GetUserDataByToken(token);
                //user = _context.Users.Include(p => p.Permissions).ThenInclude(pp => pp.UserPermissionDetail).FirstOrDefault(x => x.Id == userVM.Id);


                var xx1 = _context.Users.Include(p => p.Permissions).ThenInclude(pp => pp.UserPermissionDetail).AsQueryable();
                var xx2 = xx1.Include(x => x.Permissions).ThenInclude(xx => xx.RawPermission).AsQueryable();
                var xx3 = xx2.Include(xx => xx.Permissions).ThenInclude(xx2 => xx2.UserPermissionDetail).ThenInclude(xx3 => xx3.RawPermissionDetail);
                user = xx3.FirstOrDefault(x => x.Id == userVM.Id);

                tempUser = RetypeUser(user);
                //user = ClearSensitiveData(user);
                response.Status = true;
                response.Data = _mapper.Map<UserViewModel>(tempUser);
                return response;
            }

            response.Status = false;
            response.Data = null;
            response.Message = "Użytkownik nie jest zalogowany";
            return response;
        }

        private User RetypeUser(User dbUser)
        {
            User newUser = new User() { };

            newUser.Id = dbUser.Id;
            newUser.Name = dbUser.Name;
            newUser.Surname = dbUser.Surname;
            newUser.Login = dbUser.Login;
            newUser.BirthDate = dbUser.BirthDate;
            newUser.Gender = dbUser.Gender;
            newUser.Email = dbUser.Email;
            newUser.IsConfirmed = dbUser.IsConfirmed;
            newUser.Type = dbUser.Type;
            newUser.Role = dbUser.Role;
            newUser.Country = dbUser.Country;
            newUser.City = dbUser.City;
            newUser.ZipCode = dbUser.ZipCode;
            newUser.Street = dbUser.Street;
            newUser.BuildingNumber = dbUser.BuildingNumber;
            newUser.LocalNumber = dbUser.LocalNumber;

            newUser.Permissions = dbUser.Permissions;


            return newUser;

    }

        private User ClearSensitiveData(User user)
        {
            user.Password = null;
            user.Salt = null;

            return user;
        }

        private string GenerateJSOWebToken(UserViewModel user)
        {
             var x = _config["Jwt:Key"];
            var y = Encoding.UTF8.GetBytes(x);
            var securityKey = new SymmetricSecurityKey(y);
            var creditentals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userType", user.Type.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("userId", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creditentals);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        private string HashPassword(byte[] salt, string password)
        {
            byte[] passAsByte = Encoding.ASCII.GetBytes(password);
            var hashedPassByte = ComputeHMAC_SHA256(passAsByte, salt);
            string hashedPassString = Encoding.UTF8.GetString(hashedPassByte, 0, hashedPassByte.Length);
            return hashedPassString;
        }

        private static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }
    }





}
