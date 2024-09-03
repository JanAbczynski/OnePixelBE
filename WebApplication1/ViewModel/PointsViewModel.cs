using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.ViewModel
{
    public class PointsViewModel
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public bool Special { get; set; }
        public bool AllowToRemove { get; set; }
    }
}
