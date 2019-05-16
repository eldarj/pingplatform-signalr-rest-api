using System;
using System.Collections.Generic;
using System.Text;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class DataSpaceMetadata
    {
        public string DiskSize { get; set; } = "5GB"; // TODO: get full size of all files
        public List<NodeDto> Nodes { get; set; }
    }
}
