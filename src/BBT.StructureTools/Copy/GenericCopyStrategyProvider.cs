namespace BBT.StructureTools.Copy
{
    using BBT.StrategyPattern;
    using BBT.StructureTools.Copy.Strategy;

    /// <inheritdoc/>
    internal class GenericCopyStrategyProvider<TStrategy, TCriterion>
        : GenericStrategyProvider<TStrategy, TCriterion>,
        ICopyStrategyProvider<TStrategy, TCriterion>
        where TStrategy : ICopyStrategy<TCriterion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyStrategyProvider{TStrategy, TCriterion}"/> class.
        /// </summary>
        public GenericCopyStrategyProvider(IStrategyLocator<TStrategy> strategyLocator)
            : base(strategyLocator)
        {
        }
    }
}