// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// <see cref="ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation{T,TStrategy,TChild}"/>.
    /// </summary>
    /// <typeparam name="T">c aboF.</typeparam>
    /// <typeparam name="TStrategy">c aboF.</typeparam>
    /// <typeparam name="TChild">c aboF.</typeparam>
    public class CopyOperationCreateToOneWithGenericStrategyWithReverseRelation<T, TStrategy, TChild> : ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>
        where T : class
        where TStrategy : class, ICopyStrategy<TChild>
        where TChild : class
    {
        private readonly ICopyStrategyProvider<TStrategy, TChild> mStrategyProvider;
        private Func<T, TChild> mSourceFunc;
        private Expression<Func<T, TChild>> mTargetExpression;
        private Expression<Func<TChild, T>> mReverseRelationExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCreateToOneWithGenericStrategyWithReverseRelation{T, TStrategy, TChild}"/> class.
        /// </summary>
        public CopyOperationCreateToOneWithGenericStrategyWithReverseRelation(ICopyStrategyProvider<TStrategy, TChild> aGenericStrategyProvider)
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
            var lSource = this.mSourceFunc.Invoke(source);

            // if the source is null, set the target also to null and exit copy process step.
            if (lSource == null)
            {
                target.SetPropertyValue(this.mTargetExpression, null);
                return;
            }

            var lStrategy = this.mStrategyProvider.GetStrategy(lSource);

            var lCopy = lStrategy.Create();
            lStrategy.Copy(lSource, lCopy, copyCallContext);
            lCopy.SetPropertyValue(this.mReverseRelationExpression, target);

            target.SetPropertyValue(this.mTargetExpression, lCopy);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation{T,TStrategy,TChild}"/>.
        /// </summary>
        public void Initialize(Func<T, TChild> sourceFunc, Expression<Func<T, TChild>> targetExpression, Expression<Func<TChild, T>> aReverseRelationExpression)
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