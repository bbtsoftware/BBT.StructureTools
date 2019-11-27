namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using FluentAssertions;

    public class TreeMasterDataToTreeConverterRegistrations : IConverterRegistrations<TreeMasterData, TargetTree, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<TreeMasterData, TargetTree> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterCopyAttribute(x => x.TreeMasterDataName, x => x.TreeMasterDataName);
        }
    }
}
