// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;

    /// <summary>
    /// Strategy to convert into a target type which is unknown at compile time.
    /// </summary>
    /// <typeparam name="TBaseSource">Source base type.</typeparam>
    /// <typeparam name="TBaseTarget">Target type.</typeparam>
    /// <typeparam name="TIntention">Intention to give context of the required conversion.</typeparam>
    public interface ITargetConvertStrategy<TBaseSource, TBaseTarget, TIntention> : IGenericStrategy<TBaseTarget>
        where TIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Converts the source object to the target object.
        /// </summary>
        void Convert(TBaseSource source, TBaseTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}