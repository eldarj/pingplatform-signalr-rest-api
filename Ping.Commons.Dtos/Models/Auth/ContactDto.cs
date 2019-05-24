﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Auth
{
    public class ContactDto
    {
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public int ContactAccountId { get; set; }
    }
}
