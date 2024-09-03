using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Services
{
    public class CodeService
    {
        readonly AppDbContext _context;
        public CodeService(AppDbContext context)
        {
            _context = context;
        }

        internal CustomResponse TryToVerifyCode(String code, UserViewModel user = null)
        {
            var codeFromDB = _context.Codes.FirstOrDefault(x => x.RawCode == code);
            if (codeFromDB == null)
            {
                return new CustomResponse() { Status = false, Message = "Podany kod weryikacyjny nie istnieje" };
            }

            CustomResponse codeResponse = new CustomResponse();

            switch (codeFromDB.codeType)
            {
                case CodeType.changePassword:
                    codeResponse = ChangePassByCode(codeFromDB, user);

                    break;
                case CodeType.emailVeryfication:

                    codeResponse = EmailVeryfication(codeFromDB);
                    break;
                case CodeType.increaseCurrency:

                    break;
                case CodeType.changeEmail:

                    codeResponse = ChangeEmailVeryfication(codeFromDB);
                    break;
                default:

                    break;
            }

            if (!codeResponse.Status)
            {
                return new CustomResponse() { Status = false, Message = codeResponse.Message };
            }

            _context.SaveChanges();

            return codeResponse;
            //   return new CustomResponse() { Status = true, Message = "Kod zweryfikowany" };
        }

        private CustomResponse ChangePassByCode(Code codeFromDB, UserViewModel userVM)
        {
            CustomResponse response = new CustomResponse() { };
            response = CommonVeryfication(codeFromDB);

            if (response.Status)
            {
                response.Data = codeFromDB;
            }
            

            return response;
          
        }

        private CustomResponse ChangeEmailVeryfication(Code codeFromDB)
        {
            CustomResponse response = new CustomResponse() { };
            User user = _context.Users.FirstOrDefault(x => x.Id == codeFromDB.ConnectedUserId);
            response = CommonVeryfication(codeFromDB, user);
            if (response.Status)
            {
                user.Email = codeFromDB.AdditionalInfo;
                UseCode(codeFromDB, user);
                _context.SaveChanges();
                response.Message = "Adres email został zmieniony";
                return response;
            }
            else
            {
                return response;
            }
        }

        private CustomResponse EmailVeryfication(Code codeFromDB, User user = null)
        {
            CustomResponse response = new CustomResponse() { Status = false, Message = "Rejestracja przebiegła pomyślnie. Teraz możesz się zalogować" };
            if (codeFromDB.ConnectedUserId == null)
            {
                response.Message = "Kod jest uszkodzony";
                return response;
            }

            user = _context.Users.FirstOrDefault(x => x.Id == codeFromDB.ConnectedUserId);

            if (user == null)
            {
                response.Message = "Nie odnalziono użytkownika";
                return response;
            }
            if (user.IsConfirmed)
            {
                response.Message = "Użytkownik był już aktywowany";
                return response;
            }
            response = CommonVeryfication(codeFromDB, user);
            if (!response.Status)
            {
                return response;
            }


            user.IsConfirmed = true;
            codeFromDB = UseCode(codeFromDB, user);

            return response;
        }

        private CustomResponse CommonVeryfication(Code codeFromDB, User user = null)
        {
            CustomResponse response = new CustomResponse() { Status = true, Message = "Ok" };
            if (!codeFromDB.IsActive)
            {
                response.Status = false;
                response.Message = "Podany kod nie jest aktywny";
                return response;
            }
            if (codeFromDB.WasUsed)
            {
                response.Status = false;
                response.Message = "Podany kod został już użyty";
                return response;
            }
            if (codeFromDB.ExpireDate < DateTime.Now)
            {
                response.Status = false;
                response.Message = "Skończyła się ważność wprowadzonego kodu";
                return response;
            }
            if (codeFromDB.codeType != CodeType.changePassword &&                
                (codeFromDB.ConnectedUserId != null && codeFromDB.ConnectedUserId != user?.Id))
            {
                response.Status = false;
                response.Message = "Kod jest przypisany do innego użytkownika";
                return response;
            }

            return response;
        }

        private Code UseCode(Code codeFromDB, User user = null)
        {
            codeFromDB.DateOfUsage = DateTime.Now;
            codeFromDB.WasUsed = true;
            codeFromDB.IsActive = false;
            codeFromDB.Beneficient = user?.Id;

            return codeFromDB;
        }

    }
}
