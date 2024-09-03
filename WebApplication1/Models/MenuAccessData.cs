using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class MenuAccessData
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
        public UserType Type { get; set; }
        public string SideMenuName { get; set; }

    }
}
