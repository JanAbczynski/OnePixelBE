using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.ViewModel
{
    public class OneRangeIndexViewModel
    {
        public Guid Id { get; set; }
        public Guid ShootingRangeId { get; set; }
        public string ShootingRangeName { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public string GunsAsJson { get; set; }
        public int NoOfTargets { get; set; }
    }
}
