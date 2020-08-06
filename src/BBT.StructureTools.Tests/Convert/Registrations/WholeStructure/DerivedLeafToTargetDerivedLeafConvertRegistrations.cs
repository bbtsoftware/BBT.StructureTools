// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class DerivedLeafToTargetDerivedLeafConvertRegistrations
        : IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceDerivedLeaf, TargetDerivedLeaf> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId);
        }
    }
}
