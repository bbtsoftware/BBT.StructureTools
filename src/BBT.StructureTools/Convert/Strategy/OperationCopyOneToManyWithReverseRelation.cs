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
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TReverseRelation">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationCopyOneToManyWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>
        : IOperationCopyOneToManyWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class, TReverseRelation
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper mConvertHelper;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> mCreateConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TSourceValue> mSourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> mTargetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyOneToManyWithReverseRelation{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyOneToManyWithReverseRelation(
            IConvertHelper aConvertHelper)
        {
            aConvertHelper.Should().NotBeNull();

            this.mConvertHelper = aConvertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCreateToManyWithReverseRelation{TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> aCreateConvertHelper)
        {
            sourceFunc.Should().NotBeNull();

            targetExpression.Should().NotBeNull();

            aCreateConvertHelper.Should().NotBeNull();

            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
            this.mCreateConvertHelper = aCreateConvertHelper;
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
            additionalProcessings.Should().NotBeNull();

            var lSourceValue = this.mSourceFunc.Invoke(source);
            var lCopies = new List<TTargetValue>();

            if (!this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                lSourceValue, additionalProcessings))
            {
                return;
            }

            var lCopy = this.mCreateConvertHelper.CreateTarget(
                lSourceValue,
                target,
                additionalProcessings);
            lCopies.Add(lCopy);

            target.AddRangeToCollectionFilterNullValues(this.mTargetExpression, lCopies);
        }
    }
}
