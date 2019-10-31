// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// <see cref="ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation{T,TStrategy,TChild}"/>.
    /// </summary>
    /// <typeparam name="T">c aboF.</typeparam>
    /// <typeparam name="TStrategy">c aboF.</typeparam>
    /// <typeparam name="TChild">c aboF.</typeparam>
    public class CopyOperationCreateToManyWithGenericStrategyWithReverseRelation<T, TStrategy, TChild> : ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>
        where T : class
        where TStrategy : class, ICopyStrategy<TChild>
        where TChild : class
    {
        private readonly ICopyStrategyProvider<TStrategy, TChild> strategyProvider;
        private Func<T, IEnumerable<TChild>> sourceFunc;
        private Expression<Func<T, ICollection<TChild>>> targetexpression;
        private Expression<Func<TChild, T>> reverseRelationExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCreateToManyWithGenericStrategyWithReverseRelation{T, TStrategy, TChild}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategyWithReverseRelation(
            ICopyStrategyProvider<TStrategy, TChild> strategyProvider)
        {
            strategyProvider.Should().NotBeNull();

            this.strategyProvider = strategyProvider;
        }

        /// <summary>
        /// <see cref="ICopyOperation{T}"/>.
        /// </summary>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            var newKids = new List<TChild>();

            foreach (var child in this.sourceFunc.Invoke(source))
            {
                var strategy = this.strategyProvider.GetStrategy(child);
                var childCopy = strategy.Create();
                strategy.Copy(child, childCopy, copyCallContext);
                childCopy.SetPropertyValue(this.reverseRelationExpression, target);
                newKids.Add(childCopy);
            }

            target.AddRangeToCollectionFilterNulvalues(this.targetexpression, newKids);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation{T,TStrategy,TChild}"/>.
        /// </summary>
        public void Initialize(Func<T, IEnumerable<TChild>> sourceFunc, Expression<Func<T, ICollection<TChild>>> targetExpression, Expression<Func<TChild, T>> reverseRelationExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            reverseRelationExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
            this.reverseRelationExpression = reverseRelationExpression;
        }
    }
}