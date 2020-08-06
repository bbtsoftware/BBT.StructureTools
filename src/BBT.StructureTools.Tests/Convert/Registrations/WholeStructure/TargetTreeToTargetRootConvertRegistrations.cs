namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TargetTreeToTargetRootConvertRegistrations : IConvertRegistrations<TargetTree, TargetRoot, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<TargetTree, TargetRoot> registrations)
        {
            StructureToolsArgumentChecks.NotNull(registrations, nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x.Id, x => x.TargetTreeId);
        }
    }
}
