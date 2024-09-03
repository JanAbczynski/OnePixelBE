using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.ViewModel
{
    public class OneRangeViewModel
    {
        public Guid? Id { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public List<string> Guns { get; set; }
        public string GunsAsJson { get; set; }
        public int NoOfTargets { get; set; }
        public string GunsFriendly { get; internal set; }
    }
}
