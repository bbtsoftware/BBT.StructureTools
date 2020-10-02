namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class LeafPostProcessingsRegistrations : IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeLeaf, TargetTreeLeaf> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations.RegisterPostProcessings(
                new GenericConvertPostProcessing<SourceTreeLeaf, TargetTreeLeaf>(
                    (x, y) => y.LeafName = "PostProcessingExpected"));
        }
    }
}
