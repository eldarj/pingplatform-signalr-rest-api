using System;
using System.Collections.Generic;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class DirectoryDto
    {
        public string DirName { get; set; }
        public string Path { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
        public string OwnerFirstname { get; set; }
        public string OwnerLastname { get; set; }
        public bool Private { get; set; } = true;
        public string ParentDirName { get; set; }

        public virtual ICollection<FileDto> Files { get; set; }
        public virtual ICollection<DirectoryDto> Directories { get; set; }
    }
}
