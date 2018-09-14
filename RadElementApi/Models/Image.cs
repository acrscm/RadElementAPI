using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public string LocalUrl { get; set; }
        public string Caption { get; set; }
        public string SourceUrl { get; set; }
    }
}
