using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.Services;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class TurboPuszkaController : ControllerBase
    {

        readonly AppDbContext _context;
        private IConfiguration _config;
        private UserService _userService;
        private CommonService _commonService;

        public TurboPuszkaController(AppDbContext context, IConfiguration config, UserService userService, CommonService commonService)
        {
            _context = context;
            _config = config;
            _userService = userService;
            _commonService = commonService;
        }


           [HttpGet]
        public IActionResult DeleteMenu(Guid menuPartId)
        {
            try
            {
                _commonService.TryDeleteMenu(menuPartId);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        public IActionResult UpdateMenu(MenuPart menuPart)
        {
            try
            {
                _commonService.TryUpdateMenu(menuPart);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }


        [HttpGet]
        public IActionResult GetEnums(string EnumType)
        {
            try
            {
                Type type = Type.GetType($"OnePixelBE.CommonStuff.{EnumType}");

                List<EnumViewModel> enumList = _commonService.EnumToList2(type);
                return Ok(JsonSerializer.Serialize(enumList));
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetMenus()
        {
            try
            {
                List<MenuPart> menus = _commonService.TryGetMenus();
                return Ok(JsonSerializer.Serialize(menus));
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserViewModel user)
        {
            try
            {
                CustomResponse result = _userService.TryLogin(user);

                if (result.Status)
                {
                    string tokenStr = result.Data.ToString();
                    return Ok(new { token = tokenStr });
                }
                else
                {
                    return BadRequest(result.Message);
                }

            }catch(Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                LoggerHandler logger = new LoggerHandler(_context);
                logger.Logg(e.Message, controllerName, actionName);
                return BadRequest("Proces logowania zakończył się błędem");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterNewUser(User user)
        {
            try
            {
                CustomResponse response = _userService.TryToRegister(user);

                if (response.Status)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(response.Message);
                }

            }
            catch (Exception e)
            {
                return BadRequest("registration error");
            }

        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetAllowMenu()
        {
            try
            {
                //MenuAccessData menu = new MenuAccessData() { };
                //menu.Id = Guid.NewGuid();
                //menu.Role = UserRole.mrNobody;
                //menu.Type = UserType.person;
                //menu.SideMenuName = "Zawody";

                //_context.Add(menu);
                //_context.SaveChanges();

                GenerateMenu();

                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse response = _userService.TryGetUserData(token);

                List<string> allowMenu = _userService.TryGetAllowMenu((User)response.Data);
                return Ok(allowMenu);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetAllowMenu2()
        {
            try
            {
                //GenerateMenu();

                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse response = _userService.TryGetUserData(token);

                List<MenuOutput> allowMenu = _userService.TryGetAllowMenu2((UserViewModel)response.Data);
                return Ok(allowMenu);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        private void GenerateMenu()
        {
            MenuPart menu = new MenuPart() { };
            menu.Id = Guid.NewGuid();
            menu.UserRole = UserRole.admin;
            menu.UserType = UserType.person;
            menu.MainMenu = "Profil";
            menu.SubMenu = "Dane osobiste";
            menu.RouterLink = "/profile/data";

            _context.Add(menu);
            _context.SaveChanges();
        }


        [HttpGet]
        public IActionResult GetUserData()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse response = _userService.TryGetUserData(token);
                if (response.Status)
                {
                    response.Data = JsonSerializer.Serialize(response.Data);
                    return Ok(response.Data);
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ChangePersonalData(User user)
            {
            try
            {
                user = _userService.TryChangePersonalData(user);
                string userAsJson = JsonSerializer.Serialize(user);

                return Ok(userAsJson);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ChangeAddressData(User user)
        {
            try
            {
                user = _userService.TryChangeAddressData(user);
                string userAsJson = JsonSerializer.Serialize(user);

                return Ok(userAsJson);
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        public IActionResult ChangePassword(UserViewModel user)
        {
            try
            {
                CustomResponse customResponse = _userService.TryChangePassword(user);

                if (customResponse.Status)
                {
                    return Ok(customResponse.Message);
                }
                else
                {
                    return BadRequest(customResponse.Message);
                }
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ChangeEmail(UserViewModel user)
        {
            try
            {
                CustomResponse customResponse = _userService.TryChangeEmail(user);

                if (customResponse.Status)
                {
                    return Ok(JsonSerializer.Serialize(customResponse));
                }
                else
                {
                    return BadRequest(customResponse.Message);
                }
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult PassReset(UserViewModel user)
        {
            try
            {
                CustomResponse customResponse = _userService.TryPassReset(user);

                return Ok(JsonSerializer.Serialize(customResponse));

            }
            catch (Exception e)
            {
                CustomResponse customResponse = new CustomResponse() { Status = true, Message = "Zmiana hasła nie powiodła się" };
                return BadRequest(JsonSerializer.Serialize(customResponse));
            }
        }

        [HttpGet]
        public IActionResult GetGender()
        {
            try
            {
                List<EnumViewModel> genderList = _userService.TryGetGender();
                return Ok(JsonSerializer.Serialize(genderList));
            }
            catch (Exception)
            {

                return BadRequest("Nie udało się pobrać danych do formularza");
            }
        }


        [HttpGet]
        public IActionResult GenDefaultPermission()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse response = _commonService.TryGenDefaultPermission();

                _context.SaveChanges();
                if (response.Status)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        //public IActionResult GetGuns()
        //{
        //    try
        //    {
        //        List<EnumViewModel> gunsList = _commonService.EnumToList<Guns>();
        //        return Ok(JsonSerializer.Serialize(gunsList));
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest("Nie udało się pobrać danych do formularza");
        //    }
        //}


    }
}
