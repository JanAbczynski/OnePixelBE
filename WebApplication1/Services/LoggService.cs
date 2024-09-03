using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.EF;
using OnePixelBE.Models;

namespace OnePixelBE.Services
{
    public class LoggService
    {
        readonly AppDbContext _context;

        public LoggService(AppDbContext context)
        {
            _context = context;
        }

        internal void Logg(
            string loggInfo,
            string controller = null,
            string action = null,
            Guid? guiltyUser = null)
        {
            Logg logg = new Logg() { };
            logg.Id = Guid.NewGuid();
            logg.Date = DateTime.Now;
            logg.Info = loggInfo;
            logg.Controller = controller;
            logg.Action = action;
            logg.GuiltyUser = guiltyUser;

             _context.Add(logg);
            _context.SaveChanges();
        }

    }
}
