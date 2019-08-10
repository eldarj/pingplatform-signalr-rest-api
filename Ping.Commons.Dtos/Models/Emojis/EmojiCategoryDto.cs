using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Emojis
{
    public class EmojiCategoryDto
    {
        public string Name { get; set; }
        public ICollection<EmojiDto> Emojis { get; set; }
    }
}
