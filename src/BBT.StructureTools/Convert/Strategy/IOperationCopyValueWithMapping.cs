namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Strategy to copy.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    /// <typeparam name="TSourceValue">The type of the source attribute to copy from.</typeparam>
    /// <typeparam name="TTargetValue">The type of the target attribute to copy to.</typeparam>
    public interface IOperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>
        : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="sourceFunc">Declares the source attribute to copy from.</param>
        /// <param name="targetExpression">Declares the target attribute to copy to.</param>
        void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression);
    }
}
