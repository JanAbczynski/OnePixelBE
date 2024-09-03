using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.Models;

namespace OnePixelBE.ViewModel
{
    public class UserPermissionDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid RawPermissionDetailId { get; set; }
        public virtual AvaliblePermissionDetailViewModel RawPermissionDetail { get; set; }
    }
}
