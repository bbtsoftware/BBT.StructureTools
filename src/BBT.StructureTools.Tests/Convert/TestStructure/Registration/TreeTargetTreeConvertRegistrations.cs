using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TreeTargetTreeConvertRegistrations : IConvertRegistrations<Tree, TargetTree, ITestConvertIntention>
    {
        private readonly IConvertHelperFactory<Leaf, TargetLeaf, TargetLeaf, ITestConvertIntention> convertHelperFactory;

        public TreeTargetTreeConvertRegistrations(IConvertHelperFactory<Leaf, TargetLeaf, TargetLeaf, ITestConvertIntention> convertHelperFactory)
        {
            this.convertHelperFactory = convertHelperFactory;
        }

        public void DoRegistrations(IConvertRegistration<Tree, TargetTree> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterCopyAttribute(aX => aX.Root, aX => aX.OriginRoot)
                .RegisterCopyAttribute(aX => aX, aX => aX.OriginTree)
                .RegisterCopyAttribute(aX => aX.TreeName, aX => aX.TreeName)
                .RegisterConvertFromSourceOnDifferentLevels<TreeMasterData, TargetTree, ITestConvertIntention>(aX => aX.TreeMasterData)
                .RegisterCreateToManyGenericWithReverseRelation(
                    aX => aX.Leafs,
                    aX => aX.TargetLeafs,
                    this.convertHelperFactory.GetConvertHelper(aX => aX.TargetTree));


        }
    }
}
