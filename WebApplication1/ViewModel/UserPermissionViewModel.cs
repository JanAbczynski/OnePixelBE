using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.Models;

namespace OnePixelBE.ViewModel
{
    public class UserPermissionViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RawPermissionID { get; set; }
        public AvaliblePermissionsViewModel RawPermission { get; set; }
        public List<UserPermissionDetailViewModel> UserPermissionDetail { get; set; }
        public DateTime ExpireDate { get; set; }
        public string DocumentNumber { get; set; }

    }
}
