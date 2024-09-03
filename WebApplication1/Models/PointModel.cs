using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class PointModel
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public bool Special { get; set; }
    }
}
