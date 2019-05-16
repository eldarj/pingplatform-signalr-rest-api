using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ping.Commons.Dtos.Models.DataSpace
{
    public class NodeDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
        public string OwnerFirstname { get; set; }
        public string OwnerLastname { get; set; }
        public bool Private { get; set; } = true;
        public string NodeType { get; set; }

        public string MimeType { get; set; }
        public virtual List<NodeDto> Nodes { get; set; }

        [JsonIgnore]
        public virtual List<NodeDto> Files { get; set; }

        [JsonIgnore]
        public virtual List<NodeDto> Directories { get; set; }
    }
}
