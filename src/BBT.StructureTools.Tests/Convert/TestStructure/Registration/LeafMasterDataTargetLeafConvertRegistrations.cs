using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class LeafMasterDataTargetLeafConvertRegistrations : IConvertRegistrations<LeafMasterData, TargetLeaf, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<LeafMasterData, TargetLeaf> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterCopyAttribute(aX => aX.LeafMasterDataName, aX => aX.LeafMasterDataName);
        }
    }
}
