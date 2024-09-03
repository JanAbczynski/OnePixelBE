using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class MenuOutput
    {
        public string MainMenu { get; set; }
        public List<SubMenuOutput> SubMenu { get; set; }
    }
}
