namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyFromGenericStrategyWithReverseRelationRegistrations : IConvertRegistrations<SourceRoot, TargetRoot, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceRoot, TargetRoot> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations.RegisterCreateToManyFromGenericStrategyWithReverseRelation<SourceBaseLeaf, TargetBaseLeaf, IForTest>(
                x => x.Leafs,
                x => x.TargetLeafs,
                x => x.TargetRoot);
        }
    }
}
