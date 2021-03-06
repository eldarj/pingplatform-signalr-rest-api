﻿using Newtonsoft.Json;
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
        public string OwnerFullname {  get { return this.OwnerFirstname + " " + this.OwnerLastname;  } }
        public bool Private { get; set; } = true;
        public string NodeType { get; set; }

        public int FileSizeInKB { get; set; }
        // DirectorySizeInKB - should this be implemented?
        public string MimeType { get; set; }
        public virtual List<NodeDto> Nodes { get; set; }

        // Collections mainly used for mapping help (File and Directory Entity models to NodeDto; Checkout Automapper profile)
        [JsonIgnore]
        public virtual List<NodeDto> Files { get; set; }

        [JsonIgnore]
        public virtual List<NodeDto> Directories { get; set; }
    }
}
