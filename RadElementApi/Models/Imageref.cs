using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Imageref
    {
        public int Id { get; set; }
        public int ElementId { get; set; }
        public string ElementValue { get; set; }
        public int ImageId { get; set; }
    }
}
