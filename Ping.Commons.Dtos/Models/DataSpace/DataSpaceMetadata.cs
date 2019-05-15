using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class DataSpaceMetadata
    {
        public string Name { get => "root"; }
        public string DiskSize { get; set; } = "5GB"; // TODO: get full size of all files
        public List<FileDto> Files { get; set; }
        public List<DirectoryDto> Directories { get; set; } 
        public List<NodeDto> Nodes { get; set; }
    }
}
