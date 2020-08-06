// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Strategy
{
    using BBT.StrategyPattern;

    /// <summary>
    /// Definition of a strategy where a specific implementation instance is created based on a
    /// specific implementation it is depending on.
    /// </summary>
    /// <typeparam name="TBaseInterface">Base interface type (on which <typeparamref name="TInterface"/> depends on.</typeparam>
    /// <typeparam name="TInterface">Base interface covering the instance which is created depending on <typeparamref name="TBaseInterface"/>.</typeparam>
    public interface ICreateByBaseAsCriterionStrategy<TBaseInterface, TInterface> : IGenericStrategy<TBaseInterface>
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="TInterface"/>.
        /// </summary>
        TInterface CreateInstance();
    }
}
