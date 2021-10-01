namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class RootToTargetRootConvertRegistrations : IConvertRegistrations<SourceRoot, TargetRoot, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<SourceTree, TargetTree, TargetTree, IForTest> treeConvertHelperFactory;
        private readonly ICreateTargetImplConvertTargetHelperFactory<MasterData, TargetMasterData, TargetMasterData, IForTest> masterDataConvertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootToTargetRootConvertRegistrations" /> class.
        /// </summary>
        public RootToTargetRootConvertRegistrations(
            ICreateTargetImplConvertTargetHelperFactory<SourceTree, TargetTree, TargetTree, IForTest> treeConvertHelperFactory,
            ICreateTargetImplConvertTargetHelperFactory<MasterData, TargetMasterData, TargetMasterData, IForTest> masterDataConvertHelperFactory)
        {
            this.treeConvertHelperFactory = treeConvertHelperFactory;
            this.masterDataConvertHelperFactory = masterDataConvertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceRoot, TargetRoot> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x, x => x.OriginRoot)
                .RegisterCopyAttribute(x => x.Name, x => x.Name)
                .RegisterCopyAttributeIfSourceNotDefault(x => x.NumberSourceDefault, x => x.NumberSourceDefault)
                .RegisterCopyAttributeIfSourceNotDefault(x => x.NumberSourceNotDefault, x => x.NumberSourceNotDefault)
                .RegisterCopyAttributeIfTargetIsDefault(x => x.NumberTargetDefault, x => x.NumberTargetDefault)
                .RegisterCopyAttributeIfTargetIsDefault(x => x.NumberTargetNotDefault, x => x.NumberTargetNotDefault)
                .RegisterCopyAttributeWithLookUp(x => x.NumberSourceDefault, x => x.NumberSourceNotDefault, x => x.NumberSourceLookedUp)
                .RegisterCopyAttributeWithLookUp(x => x.NumberSourceNotDefault, x => x.NumberSourceDefault, x => x.NumberSourceNotLookedUp)
                .RegisterCopyAttributeWithUpperLimit(x => x.NumberSourceNotDefault, x => 10, x => x.NumberLimitApplied)
                .RegisterCopyAttributeWithUpperLimit(x => x.NumberSourceDefault, x => 10, x => x.NumberLimitNotApplied)
                .RegisterCopyAttributeWithMapping(x => x.EnumValue.Source, x => x.EnumValue)
                .RegisterCopyAttribute((x, y) => GetLeafPerReferenceDate(x.Tree.Hists, y.ReferenceDate), x => x.FilteredHist)
                .RegisterCreateToOneWithReverseRelation(x => x.Tree, x => x.TargetTree, this.treeConvertHelperFactory.GetConvertHelper(x => x.TargetRoot))
                .RegisterSubConvert<RootBase, IForTest>()
                .RegisterSubConvert<RootBase, RootBase, IForTest>()
                .RegisterTargetSubConvert<RootBase, IForTest>()
                .RegisterConvertFromTargetOnDifferentLevels<TargetTree, IForTest>(x => x.TargetTree)
                .RegisterConvertFromSourceOnDifferentLevels<SourceTree, IForTest>(x => x.Tree)
                .RegisterConvertFromSourceOnDifferentLevels<SourceTree, RootBase, IForTest>(x => x.Tree)
                .RegisterCreateToOneFromGenericStrategyWithReverseRelation<SourceBaseLeaf, TargetBaseLeaf, IForTest>(
                    x => x.Leaf, x => x.TargetLeaf, x => x.TargetRoot)
                .RegisterCreateToOne(x => x.ExpectedFilteredMasterData, x => x.TargetMasterData, this.masterDataConvertHelperFactory.GetConvertHelper())
                .RegisterCopyFromMany<MasterData, IForTest>(x => x.MasterDatasToFilter);
        }

        private static SourceTreeHist GetLeafPerReferenceDate(
            IEnumerable<SourceTreeHist> temporalLeafs, DateTime referenceDate)
        {
            temporalLeafs.NotNull(nameof(temporalLeafs));
            var temporalLeaf = temporalLeafs.Single(x => x.From <= referenceDate && x.To >= referenceDate);
            return temporalLeaf;
        }
    }
}
