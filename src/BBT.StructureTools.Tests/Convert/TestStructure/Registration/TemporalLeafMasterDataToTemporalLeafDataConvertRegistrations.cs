using BBT.StructureTools.Convert;
using BBT.StructureTools.Copy;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TemporalLeafMasterDataTemporalLeafDataConvertRegistrations : IConvertRegistrations<TemporalLeafMasterData, TargetLeaf, ITestConvertIntention>
    {
        public void DoRegistrations(IConvertRegistration<TemporalLeafMasterData, TargetLeaf> aRegistrations)
        {
            aRegistrations
                .RegisterCopyAttribute(aX => aX.Id, aX => aX.TemporalDataOriginId);
        }
    }
}
