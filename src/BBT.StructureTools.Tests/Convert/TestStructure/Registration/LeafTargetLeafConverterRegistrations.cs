namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using FluentAssertions;

    public class LeafTargetLeafConverterRegistrations : IConverterRegistrations<Leaf, TargetLeaf, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<Leaf, TargetLeaf> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterCopyAttribute(x => x, x => x.OriginLeaf)
                .RegisterCopyAttribute(x => x.LeafName, x => x.LeafName)
                .RegisterConvertFromSourceOnDifferentLevels<LeafMasterData, TargetLeaf, ITestConvertIntention>(x => x.LeafMasterData.Single(leafMasterData => leafMasterData.IsDefault))
                .RegisterCopyFromTemporalData<TemporalLeafMasterData, ITestConvertIntention>(x => x.TemporalLeafMasterData, (source, target) => source.TemporalDataRefDate);
        }
    }
}
