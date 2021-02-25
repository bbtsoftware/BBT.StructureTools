namespace BBT.StructureTools.Convert.Strategy
{
    /// <summary>
    /// Strategy to convert types which are unknown at compile time.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    /// <typeparam name="TIntention">Convert intention.</typeparam>
    public interface IConvertStrategyProvider<TSource, TTarget, TIntention>
        where TIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Gets a convert strategy which is responsible (determined by Source).
        /// </summary>
        ISourceConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromSource(TSource source);

        /// <summary>
        /// Gets a convert strategy which is responsible (determined by Target).
        /// </summary>
        ITargetConvertStrategy<TSource, TTarget, TIntention> GetConvertStrategyFromTarget(TTarget target);
    }
}