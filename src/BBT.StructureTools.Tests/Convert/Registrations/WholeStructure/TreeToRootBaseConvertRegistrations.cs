namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeToRootBaseConvertRegistrations : IConvertRegistrations<SourceTree, RootBase, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, RootBase> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.TreeId);
        }
    }
}
