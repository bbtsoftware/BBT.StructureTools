namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;

    public class TemporalLeafMasterDataTemporalLeafDatconvertRegistrations : IConvertRegistrations<TemporalLeafMasterData, TargetLeaf, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<TemporalLeafMasterData, TargetLeaf> registrations)
        {
            registrations
                .RegisterCopyAttribute(x => x.Id, x => x.TemporalDataOriginId);
        }
    }
}
