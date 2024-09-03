using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class UserPermissionDetail
    {
        public Guid Id { get; set; }
        public Guid RawPermissionDetailId { get; set; }
        public virtual AvaliblePermissionDetail RawPermissionDetail { get; set; }
    }
}
