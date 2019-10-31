namespace BBT.StructureTools.Tests.Convert.TestStructure.Target
{
    using System.Collections.Generic;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;

    public class TargetTree
    {
        public TargetRoot TargetRoot { get; set; }

        public Root OriginRoot { get; set; }

        public Tree OriginTree { get; set; }

        public ICollection<TargetLeaf> TargetLeafs { get; set; } = new List<TargetLeaf>();

        public string TreeMasterDataName { get; set; }

        public string TreeName { get; set; }
    }
}
