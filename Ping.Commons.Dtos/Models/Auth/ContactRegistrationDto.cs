﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Auth
{
    public class ContactRegistrationDto
    {
        public string ContactName { get; set; }
        public int CallingCountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
