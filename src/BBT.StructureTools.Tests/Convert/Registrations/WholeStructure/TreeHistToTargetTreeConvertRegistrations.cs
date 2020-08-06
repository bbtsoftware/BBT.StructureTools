namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeHistToTargetTreeConvertRegistrations
        : IConvertRegistrations<SourceTreeHist, TargetTree, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeHist, TargetTree> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.TemporalDataOriginId)
                .RegisterConvertToMany<SourceTreeHistLeaf, TargetTreeLeaf, IForTest>(
                    x => x.TreeHistLeafs, x => x.TargetLeafs, (x, y) => x.Leaf == y.OriginLeaf);
        }
    }
}
