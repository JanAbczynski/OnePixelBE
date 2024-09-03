using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.ViewModel
{
    public class AvaliblePermissionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid PermissionId { get; set; }
        public virtual AvaliblePermissionsViewModel Permission { get; set; }
        public string Name { get; set; }
    }
}
