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
        private readonly ICopyStrategyProvider<TStrategy, TChild> mStrategyProvider;
        private Func<T, IEnumerable<TChild>> mSourceFunc;
        private Expression<Func<T, ICollection<TChild>>> mTargetExpression;
        private Expression<Func<TChild, T>> mReverseRelationExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCreateToManyWithGenericStrategyWithReverseRelation{T, TStrategy, TChild}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategyWithReverseRelation(
            ICopyStrategyProvider<TStrategy, TChild> aGenericStrategyProvider)
        {
            aGenericStrategyProvider.Should().NotBeNull();

            this.mStrategyProvider = aGenericStrategyProvider;
        }

        /// <summary>
        /// <see cref="ICopyOperation{T}"/>.
        /// </summary>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            var lNewKidsList = new List<TChild>();

            foreach (var lChild in this.mSourceFunc.Invoke(source))
            {
                var lStrategy = this.mStrategyProvider.GetStrategy(lChild);
                var lChildCopy = lStrategy.Create();
                lStrategy.Copy(lChild, lChildCopy, copyCallContext);
                lChildCopy.SetPropertyValue(this.mReverseRelationExpression, target);
                lNewKidsList.Add(lChildCopy);
            }

            target.AddRangeToCollectionFilterNullValues(this.mTargetExpression, lNewKidsList);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation{T,TStrategy,TChild}"/>.
        /// </summary>
        public void Initialize(Func<T, IEnumerable<TChild>> sourceFunc, Expression<Func<T, ICollection<TChild>>> targetExpression, Expression<Func<TChild, T>> aReverseRelationExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            aReverseRelationExpression.Should().NotBeNull();

            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
            this.mReverseRelationExpression = aReverseRelationExpression;
        }
    }
}