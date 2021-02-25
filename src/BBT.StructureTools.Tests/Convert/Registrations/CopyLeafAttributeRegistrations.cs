// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CopyLeafAttributeRegistrations : IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeLeaf, TargetTreeLeaf> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations.RegisterCopyAttribute(
                x => x.Id,
                x => x.OriginId);
        }
    }
}
