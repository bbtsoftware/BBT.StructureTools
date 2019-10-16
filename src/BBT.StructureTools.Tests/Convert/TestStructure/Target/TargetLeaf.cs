namespace BBT.StructureTools.Tests.Convert.TestStructure.Target
{
    using System;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;

    public class TargetLeaf
    {
        public TargetTree TargetTree { get; set; }

        public Leaf OriginLeaf { get; set; }

        public Guid TemporalDataOriginId { get; set; }

        public string LeafMasterDataName { get; set; }

        public string LeafName { get; set; }
    }
}
