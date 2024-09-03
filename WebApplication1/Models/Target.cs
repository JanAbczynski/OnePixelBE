using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class Target
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SizeListAsJSON { get; set; }
        //public CustomFile File { get; set; }
        public string PointsIdList { get; set; }
        public Guid OwnerId { get; set; }
        public virtual WebFile AttachmentFile { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public bool IsActive { get; set; }
    }
}

