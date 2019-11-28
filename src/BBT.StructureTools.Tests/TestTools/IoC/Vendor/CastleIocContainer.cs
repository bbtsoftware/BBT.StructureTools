namespace BBT.StructureTools.Tests.TestTools.IoC.Vendor
{
    using System;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Initialization;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /// <inheritdoc/>
    internal class CastleIocContainer : IIocContainer
    {
        private IWindsorContainer castleContainer;

        /// <inheritdoc/>
        public T GetInstance<T>()
        {
            return this.castleContainer.Resolve<T>();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            this.castleContainer = new WindsorContainer();

            var resolver = new CastleResolver(this.castleContainer);

            IocHandler.Instance.IocResolver = resolver;

            // Dependencies from BBT.StrategyPattern
            this.castleContainer.Register(Component.For(typeof(IStrategyLocator<>)).ImplementedBy(typeof(IocHandlerStrategyLocator<>)));
            this.castleContainer.Register(Component.For(typeof(IInstanceCreator<,>)).ImplementedBy(typeof(GenericInstanceCreator<,>)));

            IocHandler.Instance.DoIocRegistrations(this.RegisterSingleton, this.RegisterTransient);
        }

        /// <inheritdoc/>
        public void RegisterSingleton<TAbstraction, TImplementation>()
            where TAbstraction : class
            where TImplementation : TAbstraction
        {
            this.castleContainer.Register(Component.For<TAbstraction>().ImplementedBy<TImplementation>().LifestyleSingleton());
        }

        /// <inheritdoc/>
        public void RegisterTransient<TAbstraction, TImplementation>()
            where TAbstraction : class
            where TImplementation : TAbstraction
        {
            this.castleContainer.Register(Component.For<TAbstraction>().ImplementedBy<TImplementation>().LifestyleTransient());
        }

        /// <inheritdoc/>
        public void RegisterSingleton(Type abstraction, Type implementation)
        {
            this.castleContainer.Register(Component.For(abstraction).ImplementedBy(implementation).LifestyleSingleton());
        }

        private void RegisterTransient(Type abstraction, Type implementation)
        {
            this.castleContainer.Register(Component.For(abstraction).ImplementedBy(implementation).LifestyleTransient());
        }
    }
}
