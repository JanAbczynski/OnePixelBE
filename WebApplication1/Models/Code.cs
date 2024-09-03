using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class Code
    {
        public Guid Id { get; set; }
        public string RawCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? DateOfUsage { get; set; }
        public CodeType codeType { get; set; }
        public Guid? ConnectedUserId { get; set; }
        public Guid? Beneficient { get; set; }
        public bool IsActive { get; set; }
        public bool WasUsed { get; set; }
        public string AdditionalInfo { get; set; }


    }
}
