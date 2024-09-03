using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class AvaliblePermissionDetail
    {
        public Guid Id { get; set; }
        public Guid PermissionId { get; set; }
        public virtual AvaliblePermissions Permission { get; set; }
        public string Name { get; set; }
    }
}
