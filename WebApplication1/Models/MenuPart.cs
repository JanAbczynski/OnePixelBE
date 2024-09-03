using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class MenuPart
    {
        public Guid Id { get; set; }
        public UserType UserType { get; set; }
        public UserRole UserRole { get; set; }
        public string MainMenu { get; set; }
        public string SubMenu { get; set; }
        public string RouterLink { get; set; }

        internal MenuPart Clone()
        {
            return (MenuPart)MemberwiseClone();
        }
    }
}
