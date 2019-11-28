namespace BBT.StructureTools.Tests.TestTools.IoC.Vendor
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Initialization;
    using Castle.Windsor;

    /// <summary>
    /// <see cref="IIocResolver"/> implementation for Castle Windsor.
    /// </summary>
    internal class CastleResolver : IIocResolver
    {
        private readonly IWindsorContainer windsorContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleResolver"/> class.
        /// </summary>
        public CastleResolver(IWindsorContainer windsorContainer)
        {
            this.windsorContainer = windsorContainer;
        }

        /// <inheritdoc/>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return this.windsorContainer.ResolveAll<TService>();
        }

        /// <inheritdoc/>
        public TService GetInstance<TService>()
        {
            return this.windsorContainer.Resolve<TService>();
        }

        /// <inheritdoc/>
        public object GetInstance(Type serviceType)
        {
            return this.windsorContainer.Resolve(serviceType);
        }
    }
}
