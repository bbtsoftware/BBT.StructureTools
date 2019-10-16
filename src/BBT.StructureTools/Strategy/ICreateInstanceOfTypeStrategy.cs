namespace BBT.StructureTools.Strategy
{
    using System;
    using BBT.StrategyPattern;

    /// <summary>
    /// Strategy for creation of concrete instances for an abstract base type.
    /// Provides also the concrete type to be able to ist them (e.g. in combobox dropdown).
    /// </summary>
    /// <typeparam name="TBaseTypeIntf">Interface type of the abstract base.</typeparam>
    public interface ICreateInstanceOfTypeStrategy<TBaseTypeIntf> : IGenericStrategy<Type>
    {
        /// <summary>
        /// Gets the concrete interface type of the instances this strategy creates.
        /// </summary>
        Type ConcreteIntfType { get; }

        /// <summary>
        /// Gets the concrete implementation type of the instances this strategy creates.
        /// </summary>
        Type ConcreteImplType { get; }

        /// <summary>
        /// Creates a concrete instance, see also <see cref="IInstanceCreator{TInterface, TClass}.Create"/>.
        /// </summary>
        TBaseTypeIntf CreateInstance();
    }
}
