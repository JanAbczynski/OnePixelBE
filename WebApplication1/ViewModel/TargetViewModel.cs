using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.Models;

namespace OnePixelBE.ViewModel
{
    public class TargetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SizeViewModel> Size { get; set; }
        //public CustomFileViewModel File { get; set; }
        public List<PointsViewModel> Points { get; set; }
        public string FriendlyPoints { get; set; }
        public string PointsIdList { get; set; }
        public string SizeListAsJSON { get; set; }
        public WebFile AttachmentFile { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public bool AllowToChange { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public Guid OwnerId { get; set; }
    }
}
