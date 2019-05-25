using Ping.Commons.Dtos.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Auth
{
    public class ContactDto
    {
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int AccountId { get; set; }
        public string PhoneNumber { get; set; }
        public int ContactAccountId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string AvatarImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public ICollection<MessageDto> Messages { get; set; }
    }
}
