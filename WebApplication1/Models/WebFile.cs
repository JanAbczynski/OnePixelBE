using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class WebFile
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; }
        public string NewName { get; set; }
        public string Folder { get; set; }
        public string Extension { get; set; }
        public FileStatus FileStatus { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public bool isDeleted { get; set; }
        public bool isTemp { get; set; }
    }
}
