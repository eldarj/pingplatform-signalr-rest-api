using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Emojis
{
    public class EmojiDto
    {
        public string Name { get; set; }
        public string Shortcode { get; set;  }
        public string HexCodePoint { get; set; }
        public string Category { get; set; }
    }
}
