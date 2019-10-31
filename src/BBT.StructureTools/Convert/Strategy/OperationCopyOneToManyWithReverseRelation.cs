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
        private readonly IConvertHelper convertHelper;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyOneToManyWithReverseRelation{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public OperationCopyOneToManyWithReverseRelation(
            IConvertHelper convertHelper)
        {
            convertHelper.Should().NotBeNull();

            this.convertHelper = convertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCreateToManyWithReverseRelation{TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
        {
            sourceFunc.Should().NotBeNull();

            targetExpression.Should().NotBeNull();

            createConvertHelper.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
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
            additionalProcessings.Should().NotBeNull();

            var sourceValue = this.sourceFunc.Invoke(source);
            var copies = new List<TTargetValue>();

            if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                sourceValue, additionalProcessings))
            {
                return;
            }

            var copy = this.createConvertHelper.CreateTarget(
                sourceValue,
                target,
                additionalProcessings);
            copies.Add(copy);

            target.AddRangeToCollectionFilterNulvalues(this.targetExpression, copies);
        }
    }
}
