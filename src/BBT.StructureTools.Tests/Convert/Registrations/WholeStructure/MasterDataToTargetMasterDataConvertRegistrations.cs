namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class MasterDataToTargetMasterDataConvertRegistrations : IConvertRegistrations<MasterData, TargetMasterData, IForTest>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<MasterData, TargetMasterData> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId)
                .RegisterCopyAttribute(x => x.IsDefault, x => x.IsDefault);
        }
    }
}
