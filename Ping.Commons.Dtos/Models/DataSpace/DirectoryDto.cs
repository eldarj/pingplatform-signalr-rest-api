using System;
using System.Collections.Generic;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class DirectoryDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public bool Private { get; set; } = true;

        public virtual ICollection<FileDto> Files { get; set; } // Check if we need this when creating new dirs (uploading new files)
        public virtual ICollection<DirectoryDto> Directories { get; set; }
    }
}
