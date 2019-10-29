using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TreeMasterDataToTreeConvertRegistrations : IConvertRegistrations<TreeMasterData, TargetTree, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<TreeMasterData, TargetTree> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterCopyAttribute(aX => aX.TreeMasterDataName, aX => aX.TreeMasterDataName);
        }
    }
}
