namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy.Strategy;

    /// <summary>
    /// Used to copy values using a strategy with a reverse relation.
    /// </summary>
    /// <typeparam name="T">type to copy.</typeparam>
    /// <typeparam name="TStrategy">strategy used to copy children.</typeparam>
    /// <typeparam name="TChild">type of the child.</typeparam>
    public interface ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<T, TStrategy, TChild> : ICopyOperation<T>
        where T : class
        where TStrategy : class, ICopyStrategy<TChild>
        where TChild : class
    {
        /// <summary>
        /// Initializes this operation.
        /// </summary>
        void Initialize(
            Func<T, TChild> sourceFunc,
            Expression<Func<T, TChild>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationExpression);
    }
}