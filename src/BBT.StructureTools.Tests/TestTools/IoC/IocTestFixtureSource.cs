namespace BBT.StructureTools.Tests.TestTools.IoC
{
    using BBT.StructureTools.Tests.TestTools.IoC.Vendor;

    /// <summary>
    /// Class is internally used to provide
    /// different IoC containers to test fixtures where
    /// needed. See also https://github.com/nunit/docs/wiki/TestFixtureSource-Attribute.
    /// </summary>
    internal class IocTestFixtureSource
    {
        /// <summary>
        /// Gets <see cref="IIocContainer"/> implementations
        /// for the tested IoC containers.
        /// </summary>
        internal static object[] IocContainers { get; } = 
        {
            new NinjectIocContainer(),
            new CastleIocContainer(),
        };
    }
}
