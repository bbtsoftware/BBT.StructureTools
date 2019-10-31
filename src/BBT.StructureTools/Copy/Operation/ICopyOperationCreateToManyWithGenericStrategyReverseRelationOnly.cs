// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy.Strategy;

    /// <summary>
    /// Used to copy values using a strategy with a reverse relation only.
    /// </summary>
    /// <typeparam name="T">type to copy.</typeparam>
    /// <typeparam name="TStrategy">strategy used to copy children.</typeparam>
    /// <typeparam name="TChild">type of the child.</typeparam>
    public interface ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<T, TStrategy, TChild> : ICopyOperation<T>
        where T : class
        where TStrategy : class, ICopyStrategy<TChild>
        where TChild : class
    {
        /// <summary>
        /// Initializes this operation.
        /// </summary>
        void Initialize(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> reverseRelationExpression);
    }
}