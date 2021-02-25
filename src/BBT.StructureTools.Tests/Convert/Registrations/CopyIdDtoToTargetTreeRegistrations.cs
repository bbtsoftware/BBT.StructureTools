// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CopyIdDtoToTargetTreeRegistrations : IConvertRegistrations<IdDto, TargetTree, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<IdDto, TargetTree> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations.RegisterCopyAttribute(
                x => x.Id,
                x => x.TemporalDataOriginId);
        }
    }
}
