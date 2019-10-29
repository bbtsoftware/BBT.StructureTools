using System.Collections.Generic;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Target
{
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
