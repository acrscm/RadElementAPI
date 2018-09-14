using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Code
    {
        public int Id { get; set; }
        public string Code1 { get; set; }
        public string System { get; set; }
        public string Display { get; set; }
        public DateTime AccessionDate { get; set; }
    }
}
