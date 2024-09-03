using System;

namespace OnePixelBE.ViewModel
{
    public class FieldViewModel
    {
        public Guid? Id { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public string Color { get; set; }
    }
}
