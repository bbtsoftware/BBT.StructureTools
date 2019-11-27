namespace BBT.StructureTools.Tests.TestTools
{
    using System;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Initialization;
    using BBT.StructureTools.Tests.TestTools.IoC;
    using Ninject;

    /// <inheritdoc />
    internal class NinjectIocContainer : IIocContainer
    {
        private StandardKernel ninjectKernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectIocContainer"/> class.
        /// </summary>
        public NinjectIocContainer()
        {
            this.Initialize();
        }

        /// <inheritdoc/>
        public T GetInstance<T>()
        {
            return this.ninjectKernel.Get<T>();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            var settings = new NinjectSettings
            {
                InjectNonPublic = true,
            };

            this.ninjectKernel = new StandardKernel(settings);

            var resolver = new NinjectResolver(this.ninjectKernel);
            IocHandler.Instance.IocResolver = resolver;

            // Dependencies from BBT.StrategyPattern
            this.ninjectKernel.Bind(typeof(IStrategyLocator<>)).To(typeof(NinjectStrategyLocator<>));
            this.ninjectKernel.Bind(typeof(IInstanceCreator<,>)).To(typeof(GenericInstanceCreator<,>));

            IocHandler.Instance.DoIocRegistrations(this.RegisterSingleton);
        }

        /// <inheritdoc/>
        public void RegisterSingleton<TAbstraction, TImplementation>()
            where TImplementation : TAbstraction
        {
            this.ninjectKernel.Bind<TAbstraction>().To<TImplementation>().InSingletonScope();
        }

        /// <inheritdoc/>
        public void RegisterTransient<TAbstraction, TImplementation>()
            where TImplementation : TAbstraction
        {
            this.ninjectKernel.Bind<TAbstraction>().To<TImplementation>().InTransientScope();
        }

        /// <inheritdoc/>
        public void RegisterSingleton(Type abstraction, Type implementation)
        {
            this.ninjectKernel.Bind(abstraction).To(implementation).InSingletonScope();
        }
    }
}
