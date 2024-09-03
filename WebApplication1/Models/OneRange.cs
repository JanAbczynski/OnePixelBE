using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class OneRange
    {
        public Guid Id { get; set; }
        public virtual Guid ShootingRangeId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public string GunsAsJson { get; set; }
        public int NoOfTargets { get; set; }
    }
}
