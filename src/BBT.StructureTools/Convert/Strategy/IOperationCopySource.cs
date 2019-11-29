namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Strategy to copy <typeparamref name="TSource"/> into corresponding property of <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public interface IOperationCopySource<TSource, TTarget> : IConvertOperation<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Initializes the <see cref="IConvertOperation{TSource,TTarget}"/>.
        /// </summary>
        /// <param name="targetExpression">Declares the target property.</param>
        void Initialize(Expression<Func<TTarget, TSource>> targetExpression);
    }
}
