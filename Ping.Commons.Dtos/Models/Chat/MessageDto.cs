using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Chat
{
    public class MessageDto
    {
        public string Text { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public long Ticks { get; set; }
    }
}
