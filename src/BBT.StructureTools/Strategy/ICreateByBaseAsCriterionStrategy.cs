namespace BBT.StructureTools.Strategy
{
    using BBT.StrategyPattern;

    /// <summary>
    /// Definition of a strategy where a specific implementation of the target is instantiated based
    /// on the specific source's type. This is intended to be used where a source and a target property
    /// may have multiple specific implementations inheriting from a base class or interface which isn't
    /// the same base for source and target.
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
