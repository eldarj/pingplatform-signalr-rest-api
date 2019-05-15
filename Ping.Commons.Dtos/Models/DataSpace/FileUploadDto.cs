using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class FileUploadDto
    {
        public string OwnerPhoneNumber { get; set; }
        public string OwnerFirstname { get; set; }
        public string OwnerLastname { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
        public DateTime LastAccessTime { get; set; } = DateTime.Now;
    }
}
