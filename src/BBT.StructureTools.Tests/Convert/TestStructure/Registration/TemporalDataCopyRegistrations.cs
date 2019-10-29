using BBT.StructureTools.Copy;
using BBT.StructureTools.Copy.Helper;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TemporalDataCopyRegistrations : ICopyRegistrations<ITemporalData>
    {
        public void DoRegistrations(ICopyHelperRegistration<ITemporalData> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterAttribute(aX => aX.Begin)
                .RegisterAttribute(aX => aX.End);
        }
    }
}
