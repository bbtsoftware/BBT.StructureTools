// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <summary>
    /// See <see cref="IConvertStrategyProvider{TSource, TTarget, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See above.</typeparam>
    /// <typeparam name="TTarget">See above.</typeparam>
    /// <typeparam name="TIntention">See above.</typeparam>
    public class ConvertStrategyProvider<TSource, TTarget, TIntention> : IConvertStrategyProvider<TSource, TTarget, TIntention>
        where TIntention : IBaseConvertIntention
    {
        /// <summary>
        /// See <see cref="IConvertStrategyProvider{TSource, TTarget, TIntention}.GetConvertStrategyFromSource(TSource)"/>.
        /// </summary>
        public ISourceConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromSource(TSource source)
        {
            var lStrategies = IocHandler.Instance.IocResolver.GetAllInstances<ISourceConvertStrategy<TSource, TTarget, TIntention>>();

            return lStrategies.SingleWithExceptionMessage(aX => aX.IsResponsible(source), "No convert strategy for these parameters found.");
        }

        /// <summary>
        /// See <see cref="IConvertStrategyProvider{TSource, TTarget, TIntention}.GetConvertStrategyFromTarget(TTarget)"/>.
        /// </summary>
        public ITargetConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromTarget(TTarget target)
        {
            var lStrategies = IocHandler.Instance.IocResolver.GetAllInstances<ITargetConvertStrategy<TSource, TTarget, TIntention>>();

            return lStrategies.SingleWithExceptionMessage(aX => aX.IsResponsible(target), "No convert strategy for these parameters found.");
        }
    }
}