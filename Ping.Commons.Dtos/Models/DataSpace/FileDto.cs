using System;
using System.Collections.Generic;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class FileDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public bool Private { get; set; } = true;
        public string MimeType { get; set; }
        public int FileSizeInKB { get; set; }
    }
}
