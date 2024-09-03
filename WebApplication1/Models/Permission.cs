using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string PermissionName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentParameter { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
