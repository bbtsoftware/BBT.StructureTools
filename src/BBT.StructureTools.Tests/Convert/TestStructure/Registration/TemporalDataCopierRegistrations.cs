namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using FluentAssertions;

    public class TemporalDataCopierRegistrations : ICopierRegistrations<ITemporalData>
    {
        public void DoRegistrations(ICopyHelperRegistration<ITemporalData> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterAttribute(x => x.Begin)
                .RegisterAttribute(x => x.End);
        }
    }
}
