namespace BBT.StructureTools.Tests.Convert.TestStructure.Source
{
    using System.Collections.Generic;

    public class Tree
    {
        public Root Root { get; set; }

        public TreeMasterData TreeMasterData { get; set; }

        public ICollection<Leaf> Leafs { get; set; } = new List<Leaf>();

        public string TreeName { get; set; }
    }
}
