namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeHistLeafToTargetTreeLeafConvertRegistrations
        : IConvertRegistrations<SourceTreeHistLeaf, TargetTreeLeaf, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeHistLeaf, TargetTreeLeaf> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.TreeHistLeafId, x => x.OriginTreeHistLeafId);
        }
    }
}
