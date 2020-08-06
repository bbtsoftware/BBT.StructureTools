// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Used for test purpose.
    /// </summary>
    public class EnumConvertValueRegistrations : IConvertValueRegistrations<SourceEnum, TargetEnum>
    {
        /// <summary>
        /// See <see cref="IConvertValueRegistrations{TSource, TTarget}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertValueRegistration<SourceEnum, TargetEnum> aRegistration)
        {
            StructureToolsArgumentChecks.NotNull(aRegistration, nameof(aRegistration));

            aRegistration
                .Register(SourceEnum.Value1, TargetEnum.Value1)
                .Register(SourceEnum.Value2, TargetEnum.Value2)
                .EndRegistrations();
        }
    }
}
