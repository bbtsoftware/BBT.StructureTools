namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
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
        private Func<TSource, TSourceValue> sourceFunc;
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
