﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.Services;

namespace OnePixelBE.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CodeController : ControllerBase
    {

        readonly AppDbContext _context;
        private CodeService _codeService;


        public CodeController(AppDbContext context, CodeService codeService)
        {
            _context = context;
            _codeService = codeService;
        }


        [HttpGet]
        public ActionResult VerifyEmail(String code)
        {
            CustomResponse response = _codeService.TryToVerifyCode(code);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }

            //var codeFromDB = _context.Codes.FirstOrDefault(x => x.RawCode == code);
            //if(codeFromDB == null)
            //{
            //    return BadRequest("Podany kod weryikacyjny nie istnieje");
            //}

            //CustomResponse codeResponse = new CustomResponse();

            //switch (codeFromDB.codeType)
            //{
            //    case CodeType.changePassword:

            //        break;
            //    case CodeType.emailVeryfication:

            //        codeResponse = EmailVeryfication(codeFromDB);
            //        break;
            //    case CodeType.increaseCurrency:

            //        break;
            //    default:

            //        break;
            //}

            //if (!codeResponse.Status)
            //{
            //    return BadRequest(codeResponse.Message);
            //}

            //_context.SaveChanges();
            //return Ok(codeResponse);
        }

        //private CustomResponse EmailVeryfication(Code codeFromDB, User user = null)
        //{
        //    CustomResponse response = new CustomResponse() { Status = false, Message = "Rejestracja przebiegła pomyślnie. Teraz możesz się zalogować" };
        //    if (codeFromDB.ConnectedUserId == null)
        //    {
        //        response.Message = "Kod jest uszkodzony";
        //        return response;
        //    }

        //    user = _context.Users.FirstOrDefault(x => x.Id == codeFromDB.ConnectedUserId);

        //    if(user == null)
        //    {
        //        response.Message = "Nie odnalziono użytkownika";
        //        return response;
        //    }
        //    if (user.IsConfirmed)
        //    {
        //        response.Message = "Użytkownik był już aktywowany";
        //        return response;
        //    }
        //    response = CommonVeryfication(codeFromDB, user);
        //    if(!response.Status)
        //    {
        //        return response;
        //    }


        //    user.IsConfirmed = true;
        //    codeFromDB = UseCode(codeFromDB, user);

        //    return response;
        //}

        //private Code UseCode(Code codeFromDB, User user = null)
        //{
        //    codeFromDB.DateOfUsage = DateTime.Now;
        //    codeFromDB.WasUsed = true;
        //    codeFromDB.IsActive = false;
        //    codeFromDB.Beneficient = user?.Id;

        //    return codeFromDB;
        //}

        //private CustomResponse CommonVeryfication(Code codeFromDB, User user = null)
        //{
        //    CustomResponse response = new CustomResponse() { Status = true, Message = "Ok" };
        //    if (!codeFromDB.IsActive)
        //    {
        //        response.Status = false;
        //        response.Message = "Podany kod nie jest aktywny";
        //        return response;
        //    }
        //    if (codeFromDB.WasUsed)
        //    {
        //        response.Status = false;
        //        response.Message = "Podany kod został już użyty";
        //        return response;
        //    }
        //    if (codeFromDB.ExpireDate < DateTime.Now)
        //    {
        //        response.Status = false;
        //        response.Message = "Skończyła się ważność wprowadzonego kodu";
        //        return response;
        //    }
        //    if (codeFromDB.ConnectedUserId != null && codeFromDB.ConnectedUserId != user?.Id)
        //    {
        //        response.Status = false;
        //        response.Message = "Kod jest przypisany do innego użytkownika";
        //        return response;
        //    }

        //    return response;
        //}
    }
}
