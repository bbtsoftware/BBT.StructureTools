using BBT.StructureTools.Tests.TestTools.IoC;
using Xunit;

namespace BBT.StructureTools.Tests.TestTools
{
    public class BaseIocFixture : IClassFixture<IocFixtureData>
    {
        private readonly IocFixtureData fixtureData;

        public BaseIocFixture(IocFixtureData fixtureData)
        {
            this.fixtureData = fixtureData;
        }

        public IocFixtureData FixtureData => fixtureData;
    }
}
