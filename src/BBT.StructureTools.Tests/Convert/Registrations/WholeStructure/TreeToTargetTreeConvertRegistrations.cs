namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeToTargetTreeConvertRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> treeLeafConvertHelperFactory;
        private readonly IConvertHelperFactory<SourceTreeHistLeaf, TargetTreeHistLeaf, TargetTreeHistLeaf, IForTest> treeHistLeafConvertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeToTargetTreeConvertRegistrations" /> class.
        /// </summary>
        public TreeToTargetTreeConvertRegistrations(
            IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> treeLeafConvertHelperFactory,
            IConvertHelperFactory<SourceTreeHistLeaf, TargetTreeHistLeaf, TargetTreeHistLeaf, IForTest> treeHistLeafConvertHelperFactory)
        {
            this.treeLeafConvertHelperFactory = treeLeafConvertHelperFactory;
            this.treeHistLeafConvertHelperFactory = treeHistLeafConvertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, TargetTree> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x, x => x.OriginTree)
                .RegisterCopyAttribute(x => x.TreeName, x => x.TreeName)
                .RegisterConvertFromSourceOnDifferentLevels<MasterData, TargetTree, IForTest>(x => x.MasterData)
                .RegisterCreateToManyWithReverseRelation(
                    x => x.Leafs,
                    x => x.TargetLeafs,
                    this.treeLeafConvertHelperFactory.GetConvertHelper(x => x.TargetTree))
                .RegisterCopyFromHist<SourceTreeHist, SourceTreeHist, IForTest>(
                    x => x.Hists, (source, target) => target.TargetRoot.ReferenceDate)
                .RegisterSubCopy<ICopy<BaseData>>()
                .RegisterMergeLevel(
                    x => x.Hists,
                    x => x.TreeHistLeafs,
                    x => x.TargetHistLeafs,
                    this.treeHistLeafConvertHelperFactory.GetConvertHelper(x => x.TargetTree));
        }
    }
}
