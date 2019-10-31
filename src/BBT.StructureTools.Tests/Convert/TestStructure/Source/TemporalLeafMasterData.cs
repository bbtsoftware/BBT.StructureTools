namespace BBT.StructureTools.Tests.Convert.TestStructure.Source
{
    using System;

    public class TemporalLeafMasterData : ITemporalData
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}
