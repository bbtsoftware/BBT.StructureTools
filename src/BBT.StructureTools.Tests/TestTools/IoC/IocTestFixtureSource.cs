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
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// Returns <see cref="IIocContainer"/> implementations
        /// for the tested IoC containers.
        /// </summary>
        internal static object[] IocContainers =
#pragma warning restore SA1401 // Fields should be private
        {
            new NinjectIocContainer(),
            new CastleIocContainer(),
        };
    }
}
