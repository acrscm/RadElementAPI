using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Elementvalue
    {
        public int Id { get; set; }
        public uint ElementId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Images { get; set; }
    }
}
