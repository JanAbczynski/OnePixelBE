using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class CrewStand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? OwnerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
