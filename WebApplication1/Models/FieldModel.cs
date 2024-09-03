using System;

namespace OnePixelBE.Models
{
    public class FieldModel
    {
        public Guid Id { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public string Color { get; set; }
    }
}
