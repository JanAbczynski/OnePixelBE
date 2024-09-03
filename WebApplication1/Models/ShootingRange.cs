using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class ShootingRange
    {
        public Guid Id { get; set; }
        public bool isDeleted { get; set; }
        public Guid? OwnerId { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public List<OneRange> OneRange { get; set; }
        public bool IsPublic { get; set; }
    }
}
