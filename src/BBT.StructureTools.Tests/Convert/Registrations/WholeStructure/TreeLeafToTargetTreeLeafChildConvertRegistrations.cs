namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeLeafToTargetTreeLeafChildConvertRegistrations : IConvertRegistrations<SourceTreeLeaf, TargetTreeLeafChild, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeLeaf, TargetTreeLeafChild> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId);
        }
    }
}
