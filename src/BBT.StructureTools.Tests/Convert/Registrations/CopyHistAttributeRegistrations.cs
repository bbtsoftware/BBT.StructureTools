// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CopyHistAttributeRegistrations : IConvertRegistrations<SourceTreeHist, TargetTreeHist, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeHist, TargetTreeHist> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId)
                .RegisterCopyAttribute(x => x.From, x => x.From)
                .RegisterCopyAttribute(x => x.To, x => x.To);
        }
    }
}
