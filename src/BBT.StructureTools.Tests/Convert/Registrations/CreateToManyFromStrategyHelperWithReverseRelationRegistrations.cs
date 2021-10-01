namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyFromStrategyHelperWithReverseRelationRegistrations : IConvertRegistrations<SourceRoot, TargetRoot, IForTest>
    {
        private ICreateConvertHelper<SourceBaseLeaf, TargetBaseLeaf, TargetRoot, IForTest> createConvertHelper;

        public CreateToManyFromStrategyHelperWithReverseRelationRegistrations(
            ICreateConvertFromStrategyHelperFactory<SourceBaseLeaf, TargetBaseLeaf, IForTest> createConvertHelperFactory)
        {
            createConvertHelperFactory.NotNull(nameof(createConvertHelperFactory));
            this.createConvertHelper = createConvertHelperFactory.GetConvertHelper(x => x.TargetRoot);
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceRoot, TargetRoot> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations.RegisterCreateToManyWithReverseRelation(
                x => x.Leafs,
                x => x.TargetLeafs,
                this.createConvertHelper);
        }
    }
}
