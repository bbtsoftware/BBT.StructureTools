using System;
using System.Collections.Generic;
using BBT.StructureTools.Initialization;
using FluentAssertions;
using Unity;

namespace BBT.StructureTools.Tests.TestTools.IoC
{
    /// <summary>
    /// A <see cref="IIocResolver"/> using Unity to be
    /// used within automated tests.
    /// </summary>

    public class UnityResolver : IIocResolver
    {
        private readonly IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            container.Should().NotBeNull();

            this.container = container;
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var resolved = this.container.ResolveAll<TService>();
            return resolved;
        }

        public TService GetInstance<TService>()
        {
            var resolved = this.container.Resolve<TService>();
            return resolved;
        }

        public object GetInstance(Type serviceType)
        {
            var resolved = this.container.Resolve(serviceType);
            return resolved;
        }
    }
}
