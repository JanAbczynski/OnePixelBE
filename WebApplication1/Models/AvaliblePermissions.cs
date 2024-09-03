using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class AvaliblePermissions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool HasEnumDetail { get; set; }
        //public List<string> PermissionDetail { get; set; }
    }
}
