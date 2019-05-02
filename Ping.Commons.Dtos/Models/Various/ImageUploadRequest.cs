using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.Various
{
    public class ImageUploadRequest
    {
        public string appId { get; set; }
        public string PhoneNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Base64Image { get; set; }
        public string FileName { get; set; } = "ping-img";
        public string FileExtension { get; set; } = "jpeg";
    }
}
