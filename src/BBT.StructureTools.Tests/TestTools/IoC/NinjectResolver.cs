namespace BBT.StructureTools.Tests.TestTools.IoC
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;
    using Ninject;

    /// <summary>
    /// A <see cref="IIocResolver"/> using Ninject to be
    /// used within automated tests.
    /// </summary>
    public class NinjectResolver : IIocResolver
    {
        private readonly IKernel kernel;

        public NinjectResolver(IKernel kernel)
        {
            kernel.Should().NotBeNull();

            this.kernel = kernel;
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var resolved = this.kernel.GetAll<TService>();
            return resolved;
        }

        public TService GetInstance<TService>()
        {
            var resolved = this.kernel.Get<TService>();
            return resolved;
        }

        public object GetInstance(Type serviceType)
        {
            var resolved = this.kernel.Get(serviceType);
            return resolved;
        }
    }
}
