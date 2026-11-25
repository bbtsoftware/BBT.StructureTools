namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    internal class TreeHistLeafToTargetTreeHistLeafConvertRegistrations
        : IConvertRegistrations<SourceTreeHistLeaf, TargetTreeHistLeaf, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeHistLeaf, TargetTreeHistLeaf> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x.TreeHistLeafId, x => x.OriginTreeHistLeafId);
        }
    }
}
