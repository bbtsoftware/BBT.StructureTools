using System.Linq;
using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class LeafTargetLeafConvertRegistrations : IConvertRegistrations<Leaf, TargetLeaf, ITestConvertIntention>
    {
        private readonly IConvertHelperFactory<TemporalLeafMasterData, TargetTemporalLeafData, TargetTemporalLeafData, ITestConvertIntention> convertHelperFactory;

        public LeafTargetLeafConvertRegistrations(IConvertHelperFactory<TemporalLeafMasterData, TargetTemporalLeafData, TargetTemporalLeafData, ITestConvertIntention> convertHelperFactory)
        {
            this.convertHelperFactory = convertHelperFactory;
        }

        public void DoRegistrations(IConvertRegistration<Leaf, TargetLeaf> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterCopyAttribute(aX => aX, aX => aX.OriginLeaf)
                .RegisterCopyAttribute(aX => aX.LeafName, aX => aX.LeafName)
                .RegisterConvertFromSourceOnDifferentLevels<LeafMasterData, TargetLeaf, ITestConvertIntention>(aX => aX.LeafMasterData.Single(aLeafMasterData => aLeafMasterData.IsDefault))
                .RegisterCreateToManyGenericWithReverseRelation(
                    aX => aX.TemporalLeafMasterData,
                    aX => aX.TargetTemporalLeafData,
                    this.convertHelperFactory.GetConvertHelper(aX => aX.TargetLeaf));


        }
    }
}
