using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class Logg
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Info { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Guid? GuiltyUser { get; set; }
    }
}
