// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IOperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TTargetValue>> targetexpression;

        /// <summary>
        /// See <see cref="IOperationCreateFromSourceWithReverseRelation{TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            targetExpression.Should().NotBeNull();
            createConvertHelper.Should().NotBeNull();

            this.targetexpression = targetExpression;
            this.createConvertHelper = createConvertHelper;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            if (source == null)
            {
                return;
            }

            var targetValue = this.createConvertHelper.CreateTarget(
                    source,
                    target,
                    additionalProcessings);

            target.SetPropertyValue(this.targetexpression, targetValue);
        }
    }
}
