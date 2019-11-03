﻿namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<T, TStrategy, TChild> : ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<T, TStrategy, TChild>
        where T : class
        where TStrategy : class, ICopyStrategy<TChild>
        where TChild : class
    {
        private readonly ICopyStrategyProvider<TStrategy, TChild> strategyProvider;
        private Func<T, IEnumerable<TChild>> sourceFunc;
        private Expression<Func<TChild, T>> reverseRelationExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly{T, TStrategy, TChild}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly(
            ICopyStrategyProvider<TStrategy, TChild> genericStrategyProvider)
        {
            genericStrategyProvider.Should().NotBeNull();

            this.strategyProvider = genericStrategyProvider;
        }

        /// <inheritdoc/>
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
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> reverseRelationExpression)
        {
            sourceFunc.Should().NotBeNull();

            reverseRelationExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.reverseRelationExpression = reverseRelationExpression;
        }
    }
}