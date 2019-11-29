namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public interface IOperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source history entries.</param>
        /// <param name="referenceDateFunc">Declares the reference date
        /// for selection of the specific history entry.</param>
        void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc);
    }
}
