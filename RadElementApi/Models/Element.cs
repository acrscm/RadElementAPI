using System;
using System.Collections.Generic;

namespace RadElementApi.Models
{
    public partial class Element
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Definition { get; set; }
        public string ValueType { get; set; }
        public int ValueSize { get; set; }
        public string ValueSet { get; set; }
        public float? ValueMin { get; set; }
        public float? ValueMax { get; set; }
        public float? StepValue { get; set; }
        public short MinCardinality { get; set; }
        public short MaxCardinality { get; set; }
        public string Unit { get; set; }
        public string Question { get; set; }
        public string Instructions { get; set; }
        public string References { get; set; }
        public string Version { get; set; }
        public DateTime VersionDate { get; set; }
        public string Synonyms { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Editor { get; set; }
    }
}
