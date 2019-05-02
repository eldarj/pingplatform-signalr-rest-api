using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DtoModels.Auth
{
    public class AccountDto
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
        public string Token { get; set; }
        public bool CreateSession { get; set; }
        public string AvatarImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
