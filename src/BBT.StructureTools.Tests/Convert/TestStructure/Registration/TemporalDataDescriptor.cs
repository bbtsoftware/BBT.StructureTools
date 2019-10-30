using System;
using BBT.StructureTools.Convert;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class TemporalDataDescriptor : ITemporalDataDescriptor<ITemporalData>
    {
        public DateTime GetBegin(ITemporalData aData)
        {
            aData.Should().NotBeNull();

            return aData.Begin;
        }

        public DateTime GetEnd(ITemporalData aData)
        {
            aData.Should().NotBeNull();

            return aData.End;
        }
    }
}
