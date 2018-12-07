using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Coderef
    {
        public int Id { get; set; }
        public int CodeId { get; set; }
        public uint ElementId { get; set; }
        public string ValueCode { get; set; }
    }
}
