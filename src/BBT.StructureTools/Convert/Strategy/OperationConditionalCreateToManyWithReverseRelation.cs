// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Strategy;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationConditionalCreateToManyWithReverseRelation{TSource, TTarget, TBaseSource, TBaseTarget, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TBaseSource">Contains the base type of the source which shall be converted (e.g. LiBaseCover).</typeparam>
    /// <typeparam name="TBaseTarget">Contains the base type of the target which shall be converted (e.g. LiClaimCover).</typeparam>
    /// <typeparam name="TIntention">Conversion intention which shall be used within the strategy.</typeparam>
    public class OperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
        : IOperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>
            where TSource : class
            where TTarget : class
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
    {
        private readonly IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> mConvertStrategyProvider;
        private readonly IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> mCreateInstanceStrategyProvider;

        private Func<TSource, IEnumerable<TBaseSource>> mSource;
        private Expression<Func<TTarget, ICollection<TBaseTarget>>> mTargetParent;
        private Expression<Func<TBaseTarget, TTarget>> mReverseRelationOnTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConditionalCreateToManyWithReverseRelation{TSource, TTarget, TBaseSource, TBaseTarget, TIntention}"/> class.
        /// </summary>
        public OperationConditionalCreateToManyWithReverseRelation(
            IConvertStrategyProvider<TBaseSource, TBaseTarget, TIntention> aConvertStrategyProvider,
            IGenericStrategyProvider<ICreateByBaseAsCriterionStrategy<TBaseSource, TBaseTarget>, TBaseSource> aCreateInstanceStrategyProvider)
        {
            aConvertStrategyProvider.Should().NotBeNull();
            aCreateInstanceStrategyProvider.Should().NotBeNull();

            this.mConvertStrategyProvider = aConvertStrategyProvider;
            this.mCreateInstanceStrategyProvider = aCreateInstanceStrategyProvider;
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy(T, T, Interfaces.Tools.Copy.ICopyCallContext)"/>.
        /// </summary>
        public void Execute(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            var lSourceChildren = this.mSource.Invoke(source);
            foreach (var lSourceChildElement in lSourceChildren)
            {
                var lInstanceCreationStrategy = this.mCreateInstanceStrategyProvider.GetStrategy(lSourceChildElement);
                var lTarget = lInstanceCreationStrategy.CreateInstance();

                var lStrategy = this.mConvertStrategyProvider.GetConvertStrategyFromSource(lSourceChildElement);
                lStrategy.Convert(lSourceChildElement, lTarget, additionalProcessings);

                // set reverse relation
                this.mReverseRelationOnTarget.Compile().Invoke(lTarget);

                // Add to target collection
                this.mTargetParent.Compile().Invoke(target).Add(lTarget);
            }
        }

        /// <summary>
        /// See <see cref="IOperationConditionalCreateToManyWithReverseRelation{TSource, TTarget, TBaseSource, TBaseTarget, TIntention}.Initialize(Func{TSource, IEnumerable{TBaseSource}}, Expression{Func{TTarget, ICollection<TChildType>}}, Expression{Func{TBaseTarget, TTarget}})"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> aReverseRelationOnTarget)
        {
            source.Should().NotBeNull();
            targetParent.Should().NotBeNull();
            aReverseRelationOnTarget.Should().NotBeNull();

            this.mSource = source;
            this.mTargetParent = targetParent;
            this.mReverseRelationOnTarget = aReverseRelationOnTarget;
        }
    }
}
