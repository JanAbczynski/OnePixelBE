using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.ViewModel
{
    public class ShootingRangeViewModel
    {
        public Guid? Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AddressViewModel Address { get; set; }
        public List<OneRangeViewModel> OneRange { get; set; }
        public bool IsPublic { get; set; }
        public bool IsEditable { get; set; }

    }
}
