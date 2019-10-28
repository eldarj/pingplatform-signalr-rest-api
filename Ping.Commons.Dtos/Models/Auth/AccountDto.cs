using Ping.Commons.Dtos.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ping.Commons.Dtos.Models.Chat;

namespace Api.DtoModels.Auth
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int CallingCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
        public string Token { get; set; }
        public bool CreateSession { get; set; }
        public string AvatarImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public ICollection<ContactDto> Contacts { get; set; }
    }
}
