using BBT.StructureTools.Convert;
using BBT.StructureTools.Copy;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TemporalLeafMasterDataTemporalLeafDataConvertRegistrations : IConvertRegistrations<TemporalLeafMasterData, TargetTemporalLeafData, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<TemporalLeafMasterData, TargetTemporalLeafData> aRegistrations)
        {
            aRegistrations
                .RegisterCopyAttribute(aX => aX.Id, aX => aX.OriginId)
                .RegisterSubCopy<ICopy<ITemporalData>>();
        }
    }
}
