namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using FluentAssertions;

    public class LeafMasterDataTargetLeafConverterRegistrations : IConverterRegistrations<LeafMasterData, TargetLeaf, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<LeafMasterData, TargetLeaf> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterCopyAttribute(x => x.LeafMasterDataName, x => x.LeafMasterDataName);
        }
    }
}
