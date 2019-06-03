using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Auth
{
    public class CallingCodeDto
    {
        public int CallingCountryCode { get; set; }
        public string CountryName { get; set; }
        public string IsoCode { get; set; }
    }
}
