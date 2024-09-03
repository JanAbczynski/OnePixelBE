using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class CustomFile
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public string OriginalName { get; set; }
        public string FileFolder { get; set; }
    }
}
