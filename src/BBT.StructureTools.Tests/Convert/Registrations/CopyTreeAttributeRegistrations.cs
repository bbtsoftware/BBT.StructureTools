namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CopyTreeAttributeRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, TargetTree> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x.TreeName, x => x.TreeName)
                .RegisterCopyAttribute((x, y) => y.MasterDataId, x => x.TemporalDataOriginId);
        }
    }
}
