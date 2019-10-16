namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">Todo.</typeparam>
    /// <typeparam name="TConvertIntention">Todo.</typeparam>
    internal interface IOperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
            where TSource : class
            where TTarget : class
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source value.</param>
        void Initialize(Func<TTarget, TSourceValue> sourceFunc);
    }
}
