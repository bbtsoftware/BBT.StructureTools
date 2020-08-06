namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeLeafToTargetTreeLeafConvertRegistrations : IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>
    {
        private readonly IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeafChild, TargetTreeLeafChild, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeLeafToTargetTreeLeafConvertRegistrations" /> class.
        /// </summary>
        public TreeLeafToTargetTreeLeafConvertRegistrations(
            IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeafChild, TargetTreeLeafChild, IForTest> convertHelperFactory)
        {
            StructureToolsArgumentChecks.NotNull(convertHelperFactory, nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeLeaf, TargetTreeLeaf> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x, x => x.OriginLeaf)
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId)
                .RegisterCopyAttribute(x => x.LeafName, x => x.LeafName)
                .RegisterConvertFromSourceOnDifferentLevels<MasterData, TargetTreeLeaf, IForTest>(
                    x => x.MasterDatas.Single(y => y.IsDefault))
                .RegisterCreateFromSourceWithReverseRelation(
                    x => x.Child,
                    this.convertHelperFactory.GetConvertHelper(x => x.TargetTreeLeaf));
        }
    }
}
