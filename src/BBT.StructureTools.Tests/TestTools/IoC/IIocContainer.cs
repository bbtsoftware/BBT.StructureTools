namespace BBT.StructureTools.Tests.TestTools.IoC
{
    using System;

    /// <summary>
    /// Abstraction of any IoC container to be used for testing.
    /// </summary>
    public interface IIocContainer
    {
        /// <summary>
        /// Initializes the IoC container.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Registers the <typeparamref name="TImplementation"/> for the given
        /// <typeparamref name="TAbstraction"/> as singleton instance.
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction (interface) type.</typeparam>
        /// <typeparam name="TImplementation">Implementation of <typeparamref name="TAbstraction"/>.</typeparam>
        void RegisterSingleton<TAbstraction, TImplementation>()
            where TAbstraction : class
            where TImplementation : TAbstraction;

        /// <summary>
        /// Registers the <typeparamref name="TImplementation"/> for the given
        /// <typeparamref name="TAbstraction"/> transiently.
        /// </summary>
        /// <typeparam name="TAbstraction">Abstraction (interface) type.</typeparam>
        /// <typeparam name="TImplementation">Implementation of <typeparamref name="TAbstraction"/>.</typeparam>
        void RegisterTransient<TAbstraction, TImplementation>()
            where TAbstraction : class
            where TImplementation : TAbstraction;

        /// <summary>
        /// Registers the <paramref name="implementation"/> for the given
        /// <paramref name="abstraction"/> as singleton instance.
        /// </summary>
        /// <param name="abstraction">Abstraction (interface) type.</param>
        /// <param name="implementation">Implementation of the abstraction.</param>
        void RegisterSingleton(Type abstraction, Type implementation);

        /// <summary>
        /// Returns the implementation registered for <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Abstraction for which the implementation is returned.</typeparam>
        T GetInstance<T>();
    }
}
