using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.Models.Common;
using OnePixelBE.Services;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Controllers
{
    [Route("[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class ShootingRangeController : ControllerBase
    {
        readonly AppDbContext _context;
        private CommonService _commonService;
        private UserService _userService;
        private ShootingRangeService _shootingRangeService;
        private LoggService _loggService;
        public ShootingRangeController(
            CommonService commonService, 
            UserService userService, 
            AppDbContext context, 
            ShootingRangeService shootingRangeService,
            LoggService loggService)
        {
            _commonService = commonService;
            _context = context;
            _shootingRangeService = shootingRangeService;
            _userService = userService;
            _loggService = loggService;
        }


        [HttpGet]
        public IActionResult GetShootingRangeData(Guid id)
        {
            try
            {
                CustomResponse response = _shootingRangeService.TryGetShootingRangeData(id);

                if (response.Status)
                {
                    return Ok(JsonSerializer.Serialize(response.Data));
                }
                else
                {
                    return BadRequest(response.Message);
                }
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }

        [HttpPost]
        public IActionResult EditShootingRange(ShootingRangeViewModel shootingRange)
        {
            try
            {
                var response = _shootingRangeService.TryEditShootingRange(shootingRange);
                if (response.Status)
                {
                    _context.SaveChanges();
                    return Ok(response.Data);

                }
                else
                {
                    return BadRequest(JsonSerializer.Serialize(response));
                }
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }


        [HttpGet]
        public IActionResult DeleteShootingRange(Guid id)
        {
            try
            {
                CustomResponse response = _shootingRangeService.TryDeleteShootingRange(id);

                _context.SaveChanges();
                if (response.Status)
                {
                    return Ok(JsonSerializer.Serialize(response.Message));
                }
                else
                {
                    return BadRequest(JsonSerializer.Serialize(response));
                }

            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);


                string messege = "Bład: " + e.Message;
                return BadRequest(JsonSerializer.Serialize(messege));
            }
        }


        [HttpPost]
        public IActionResult AddShootingRange(ShootingRangeViewModel shootingRange)
        {
            try
            {
                _shootingRangeService.TryAddShootingRange(shootingRange);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }


        [HttpPost]
        public IActionResult GetOneRangeData(List<Guid> shootingRangesId)
        {
            try
            {
                CustomResponse response = _shootingRangeService.TryGetOneRangeData(shootingRangesId);

                return Ok(JsonSerializer.Serialize(response.Data));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych strzelnicy");
            }
        }

        [HttpPost]  
        [AllowAnonymous]
        public IActionResult UploadTarget()
            {
            try
            {


                var file = Request.Form.Files[0];
                CustomResponse response = _shootingRangeService.UploadTarget(file);

                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Data));
            }
                catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się wgrać pliku");
            }
        }

        [HttpGet]
        public IActionResult SubmitTarget(TargetViewModel targetModel)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TrySubmitTarget(targetModel, (User)userResponse.Data);

                _context.SaveChanges();
                return Ok();
                //CustomResponse response = _shootingRangeService.UploadTarget(file);
                //return Ok(JsonSerializer.Serialize(response.Data));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało zapisać tarczy");
            }
        }

        [HttpGet]
        public IActionResult SetActive(Guid Id)
        {
            try
            {
     
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);
                if (!userResponse.Status)
                {
                    return BadRequest(userResponse.Message);
                }

                CustomResponse response = _shootingRangeService.TrySetActive((User)userResponse.Data, Id);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }
                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response));

            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }


        [HttpGet]
        public IActionResult GetTargets(TableParams tableParams)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);
                if(!userResponse.Status)
                {
                    return BadRequest(userResponse.Message);
                }

                CustomResponse response = _shootingRangeService.TryGetTargets((User)userResponse.Data);
                return Ok(JsonSerializer.Serialize(response.Data));

            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }

        [HttpGet]
        public IActionResult GetTarget(Guid Id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryGetTarget(Id, (User)userResponse.Data);
                return Ok(JsonSerializer.Serialize(response.Data));

            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie odnaleziono tarczy");
            }
        }

        [HttpGet]
        public IActionResult DeleteTarget(Guid Id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryDeleteTarget(Id, (User)userResponse.Data);
                if (response.Status)
                {
                    _context.SaveChanges();
                    return Ok(JsonSerializer.Serialize(response.Message));
                }
                else
                {

                    return BadRequest(JsonSerializer.Serialize(response.Message));
                }
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych");
            }
        }

        [HttpGet]
        public IActionResult CopyTarget(Guid Id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);
                if (!userResponse.Status)
                {
                    return BadRequest("Nie jesteś zalogowany");
                }

                CustomResponse response = _shootingRangeService.TryCopyTarget(Id, (User)userResponse.Data);
                if (response.Status)
                {
                    _context.SaveChanges();
                    return Ok(JsonSerializer.Serialize(response.Message));
                }
                else
                {

                    return BadRequest(JsonSerializer.Serialize(response.Message));
                }
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało skopiować tarczy");
            }
        }

        [HttpPost]
        public IActionResult EditTarget(TargetViewModel targetModel)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryEditTarget(targetModel, (User)userResponse.Data);
                if (response.Status)
                {
                    _context.SaveChanges();
                    return Ok(JsonSerializer.Serialize(response.Data));
                }
                else
                {
                    return BadRequest(response.Message);
                }
                

            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);
                return BadRequest("Nie udało się edytować celu");
            }
        }


        //public IActionResult AddTarget([FromForm] Competition competition)
        //{

        //    var path = "E:\\uploades";
        //    var file = competition.TargetImage2;
        //    var extension = Path.GetExtension(file.FileName);

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }


        //        string filePath = Path.Combine(path, $"{Guid.NewGuid().ToString()}.{extension}");
        //    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        file.CopyToAsync(fileStream);
        //    }

        //    return Ok();
        //}

        [HttpGet]
        public IActionResult RemoveTempFile(Guid Id)
        {
            try
            {
                CustomResponse resnse = _shootingRangeService.TryRemoveTempFile(Id);
                _context.SaveChanges();
                if (resnse.Status)
                {
                    return Ok();
                }
                else
                {
                    switch (resnse.ResponseCode)
                    {
                        case ResponseCode.conflict:
                            return Conflict(resnse);
                            break;
                        default :
                            return BadRequest(resnse);
                            break;

                    }
                }
                
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Błąd podczas usuwania pliku.");
            }
        }

        [HttpGet]
        public IActionResult GetOneShootingRange(Guid Id)
        {
            try
            {
                CustomResponse resnse = _shootingRangeService.TryGetOneShootingRange(Id);
                return Ok(JsonSerializer.Serialize(resnse.Data));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych strzelnicy");
            }
        }

        [HttpPost]
        public IActionResult GetShootingRanges(TableParams tableParams)
        {
            try
            {
                List<ShootingRangeIndexViewModel> shootingRangeIndex = _shootingRangeService.TryGetShootingRanges();
                return Ok(JsonSerializer.Serialize(shootingRangeIndex));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych do formularza");
            }
        }

        [HttpGet]
        public IActionResult GetGuns()
        {
            try
            {
                List<EnumViewModel> gunsList = _commonService.EnumToList<Guns>();
                return Ok(JsonSerializer.Serialize(gunsList));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych do formularza");
            }
        }


        [HttpGet]
        public IActionResult GetCrewStands()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryGetCrewStands((User)userResponse.Data);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }
                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Data));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się pobrać danych stanowisk");
            }
        }

        [HttpPost]
        public IActionResult AddCrewStand(CrewStandViewModel CrewStand)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryAddCrewStand(CrewStand, (User) userResponse.Data);
                if (!response.Status)
                {
                    if(response.ResponseCode == ResponseCode.conflict)
                    {
                        return Conflict(response.Message);
                    }
                    return BadRequest(response.Message);
                }


                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Message));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się zapisać danych stanowisk obługi");
            }
        }

        [HttpGet]
        public IActionResult DeleteCrewStand(Guid id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryDeleteCrewStand(id, (User) userResponse.Data);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }


                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Message));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się usunąć stanowiska obługi");
            }
        }


        [HttpPost]
        public IActionResult EditCrewStand(CrewStandViewModel CrewStand)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryEditCrewStand(CrewStand, (User)userResponse.Data);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }


                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Message));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się zapisać stanowiska");
            }



        }[HttpPost]
        public IActionResult RestoreCrewStand(CrewStandViewModel CrewStand)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryRestoreCrewStand(CrewStand, (User)userResponse.Data);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }

                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Message));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się przywrócić stanowiska");
            }



        }[HttpPost]
        public IActionResult ForsceAdd(CrewStandViewModel CrewStand)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                CustomResponse userResponse = _userService.TryGetUserData(token);

                CustomResponse response = _shootingRangeService.TryEditCrewStand(CrewStand, (User)userResponse.Data);
                if (!response.Status)
                {
                    return BadRequest(response.Message);
                }


                _context.SaveChanges();
                return Ok(JsonSerializer.Serialize(response.Message));
            }
            catch (Exception e)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                _loggService.Logg(e.Message, controllerName, actionName);

                return BadRequest("Nie udało się zapisać stanowiska");
            }
        }
    }
}
