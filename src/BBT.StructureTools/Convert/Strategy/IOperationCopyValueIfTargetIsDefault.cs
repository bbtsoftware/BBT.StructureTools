namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Copy operation which only copies values if they are not equivalent to the default value.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    /// <typeparam name="TValue">The type of the value to copy.</typeparam>
    public interface IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
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
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);
    }
}
