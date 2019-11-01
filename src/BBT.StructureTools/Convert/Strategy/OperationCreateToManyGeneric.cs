namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationCreateToManyGeneric<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IOperationCreateToManyGeneric<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;
        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper;
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;
        private Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetexpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCreateToManyGeneric{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCreateToManyGeneric(
            IConvertHelper convertHelper)
        {
            convertHelper.Should().NotBeNull();

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            createConvertHelper.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
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

            var sourceValues = this.sourceFunc.Invoke(source);

            var copies = new List<TTargetValue>();

            foreach (var sourceValue in sourceValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                {
                    continue;
                }

                var copy = this.createConvertHelper.CreateTarget(
                    sourceValue,
                    additionalProcessings);
                copies.Add(copy);
            }

            target.SetPropertyValue(this.targetexpression, copies);
        }
    }
}
