namespace BBT.StructureTools.Convert.Strategy
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    public class ConvertStrategyProvider<TSource, TTarget, TIntention> : IConvertStrategyProvider<TSource, TTarget, TIntention>
        where TIntention : IBaseConvertIntention
    {
        /// <inheritdoc/>
        public ISourceConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromSource(TSource source)
        {
            var strategies = IocHandler.Instance.IocResolver.GetAllInstances<ISourceConvertStrategy<TSource, TTarget, TIntention>>();

            return strategies.SingleWithExceptionMessage(x => x.IsResponsible(source), "No convert strategy for these parameters found.");
        }

        /// <inheritdoc/>
        public ITargetConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromTarget(TTarget target)
        {
            var strategies = IocHandler.Instance.IocResolver.GetAllInstances<ITargetConvertStrategy<TSource, TTarget, TIntention>>();

            return strategies.SingleWithExceptionMessage(x => x.IsResponsible(target), "No convert strategy for these parameters found.");
        }
    }
}