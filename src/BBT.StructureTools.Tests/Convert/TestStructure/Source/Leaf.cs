using System.Collections.Generic;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Source
{
    public class Leaf
    {
        public Tree Tree { get; set; }

        public ICollection<TemporalLeafMasterData> TemporalLeafMasterData { get; set; } = new List<TemporalLeafMasterData>();

        public ICollection<LeafMasterData> LeafMasterData { get; set; } = new List<LeafMasterData>();

        public string LeafName { get; set; }
    }
}
