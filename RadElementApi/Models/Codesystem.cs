using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Codesystem
    {
        public string Abbrev { get; set; }
        public string Name { get; set; }
        public string Oid { get; set; }
        public string SystemUrl { get; set; }
        public string CodeUrl { get; set; }
    }
}
