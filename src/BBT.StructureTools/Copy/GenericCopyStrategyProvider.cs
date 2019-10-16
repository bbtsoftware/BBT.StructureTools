// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy
{
    using BBT.StrategyPattern;
    using BBT.StructureTools.Copy.Strategy;

    /// <summary>
    /// <see cref="ICopyStrategyProvider{TStrategy,TCriterion}"/>.
    /// </summary>
    /// <typeparam name="TStrategy">see above.</typeparam>
    /// <typeparam name="TCriterion">see above.</typeparam>
    public class GenericCopyStrategyProvider<TStrategy, TCriterion>
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