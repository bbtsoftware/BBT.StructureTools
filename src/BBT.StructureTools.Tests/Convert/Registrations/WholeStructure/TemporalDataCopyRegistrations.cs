namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TemporalDataCopyRegistrations : ICopyRegistrations<ITemporalData>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(ICopyHelperRegistration<ITemporalData> registrations)
        {
            StructureToolsArgumentChecks.NotNull(registrations, nameof(registrations));

            registrations
                .RegisterAttribute(x => x.From)
                .RegisterAttribute(x => x.To);
        }
    }
}
