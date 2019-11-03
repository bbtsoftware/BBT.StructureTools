namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy.Strategy;

    /// <summary>
    /// Defines a copy helper strategy used to copy an i enumerable using a strategy.
    /// </summary>
    /// <typeparam name="T">typeof T type.</typeparam>
    /// <typeparam name="TStrategy">type of the strategy.</typeparam>
    /// <typeparam name="TChildType">type of the children being copied.</typeparam>
    internal interface ICopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType> : ICopyOperation<T>
        where TStrategy : class, ICopyStrategy<TChildType>
        where TChildType : class
    {
        /// <summary>
        /// Initializes the copy helper.
        /// </summary>
        /// <param name="sourceFunc">function to retrieve the children ist.</param>
        /// <param name="targetExpression">function to set the target ist.</param>
        /// <param name="createTargetChildExpression">function used to get new child instance from factory.</param>
        void Initialize(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> createTargetChildExpression);
    }
}