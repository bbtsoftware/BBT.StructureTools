// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;

    /// <summary>
    /// Generic strategy factory for criteria of type <typeparamref name="TCriterion"/>
    /// and strategies of type <typeparamref name="TStrategy"/>.
    /// </summary>
    /// <typeparam name="TStrategy">The type of strategy.</typeparam>
    /// <typeparam name="TCriterion">The type of criterium.</typeparam>
    public interface ICopyStrategyProvider<out TStrategy, in TCriterion> : IGenericStrategyProvider<TStrategy, TCriterion>
        where TStrategy : ICopyStrategy<TCriterion>
    {
    }
}
