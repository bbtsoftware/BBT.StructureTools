using System.Collections.Generic;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Target
{
    public class TargetLeaf
    {
        public TargetTree TargetTree { get; set; }

        public Leaf OriginLeaf { get; set; }

        public ICollection<TargetTemporalLeafData> TargetTemporalLeafData { get; set; }

        public string LeafMasterDataName { get; set; }

        public string LeafName { get; set; }
    }
}
