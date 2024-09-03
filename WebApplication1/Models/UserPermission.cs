using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class UserPermission
    {
        public Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public Guid RawPermissionID { get; set; }
        public virtual AvaliblePermissions RawPermission { get; set; }
        public List<UserPermissionDetail> UserPermissionDetail { get; set; }
        public DateTime ExpireDate { get; set; }
        public string DocumentNumber { get; set; }
    }
}
