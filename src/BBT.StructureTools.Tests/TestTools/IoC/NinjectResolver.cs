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
            return this.kernel.GetAll<TService>();
        }

        public TService GetInstance<TService>()
        {
            return this.kernel.Get<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return this.kernel.Get(serviceType);
        }
    }
}
