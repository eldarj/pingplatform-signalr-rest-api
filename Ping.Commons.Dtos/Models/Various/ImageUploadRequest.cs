using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Various
{
    public class ImageUploadRequest
    {
        public string Base64Image { get; set; }
        public string FileName { get; set; } = "ping-img";
        public string FileExtension { get; set; } = "jpeg";
    }
}
