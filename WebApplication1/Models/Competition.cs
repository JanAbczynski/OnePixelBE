using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Models
{
    public class Competition
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public string Name { get; set; }
        public IFormFile TargetImage { get; set; }
        public IFormFile TargetImage2 { get; set; }


        //    public startDate?: string;
        //public place?: string;
        //public name?: string;
        //public Schortcut?: string;
        //// public shootingRange?: ShootingRange;
        //public shootingRanges?: ShootingRange[];
        //public Runs?: Run[];
        //public TargetImage?: File;
    }
}
