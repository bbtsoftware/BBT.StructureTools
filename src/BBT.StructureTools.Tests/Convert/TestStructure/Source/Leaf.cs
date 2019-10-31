namespace BBT.StructureTools.Tests.Convert.TestStructure.Source
{
    using System;
    using System.Collections.Generic;

    public class Leaf
    {
        public Tree Tree { get; set; }

        public ICollection<TemporalLeafMasterData> TemporalLeafMasterData { get; set; } = new List<TemporalLeafMasterData>();

        public ICollection<LeafMasterData> LeafMasterData { get; set; } = new List<LeafMasterData>();

        public string LeafName { get; set; }

        public DateTime TemporalDataRefDate { get; set; }
    }
}
