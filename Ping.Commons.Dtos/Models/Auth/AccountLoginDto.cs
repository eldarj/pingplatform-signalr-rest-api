using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DtoModels.Auth
{
    public class AccountLoginDto
    {
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public bool CreateSession { get; set; }
    }
}
