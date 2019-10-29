using System;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Target
{
    public class TargetTemporalLeafData : ITemporalData
    {
        public Guid OriginId { get; set; }

        public TargetLeaf TargetLeaf { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}
