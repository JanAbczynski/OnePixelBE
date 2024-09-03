using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;

namespace OnePixelBE.Models
{
    public class CustomResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Message_2 { get; set; }
        public Object Data { get; set; }
        public ResponseCode ResponseCode { get; set; }
       
    }
}
