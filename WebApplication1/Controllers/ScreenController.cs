using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.Services;
using OnePixelBE.ViewModel;
using System;

namespace OnePixelBE.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        private ScreenService _screenService;

        public ScreenController(
            ScreenService screenService)
        {
            _screenService = screenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult InitScreen(FieldViewModel[][] screen)
        {
            try
            {
                CustomResponse response = _screenService.TryToInitScreen(screen);

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

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetScreen()
        {
            try
            {
                CustomResponse response = _screenService.TryToGetScreen();

                if (response.Status)
                {
                    return Ok(response);
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

            return Ok();
        }

        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetColors()
        {
            try
            {
                CustomResponse response = _screenService.TryToGetColors();

                if (response.Status)
                {
                    return Ok(response);
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

            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateOneField(FieldViewModel field)
        {
            try
            {
                CustomResponse response = _screenService.TryToUpdateOneField(field);

                if (response.Status)
                {
                    return Ok(response);
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

            return Ok();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult UpdateOneFieldWithSignal(FieldViewModel fieldVM)
        {

            var response = _screenService.TryToUpdateOneFieldAsync(fieldVM);

            return Ok();
        }
    }
}
