namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using FluentAssertions;

    public class TreeTargetTreeConverterRegistrations : IConverterRegistrations<Tree, TargetTree, ITestConvertIntention>
    {
        private readonly IConvertHelperFactory<Leaf, TargetLeaf, TargetLeaf, ITestConvertIntention> convertHelperFactory;

        public TreeTargetTreeConverterRegistrations(IConvertHelperFactory<Leaf, TargetLeaf, TargetLeaf, ITestConvertIntention> convertHelperFactory)
        {
            this.convertHelperFactory = convertHelperFactory;
        }

        public void DoRegistrations(IConvertRegistration<Tree, TargetTree> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterCopyAttribute(x => x.Root, x => x.OriginRoot)
                .RegisterCopyAttribute(x => x, x => x.OriginTree)
                .RegisterCopyAttribute(x => x.TreeName, x => x.TreeName)
                .RegisterConvertFromSourceOnDifferentLevels<TreeMasterData, TargetTree, ITestConvertIntention>(x => x.TreeMasterData)
                .RegisterCreateToManyGenericWithReverseRelation(
                    x => x.Leafs,
                    x => x.TargetLeafs,
                    this.convertHelperFactory.GetConvertHelper(x => x.TargetTree));
        }
    }
}
