namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>
        : IOperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>
        where TSource : class
        where TTarget : class
    {
        private readonly IConvertValue<TSourceValue, TTargetValue> convertValue;
        private Func<TSource, TSourceValue> sourceFunc;
        private Expression<Func<TTarget, TTargetValue>> targetexpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithMapping{TSource, TTarget, TSourceValue, TTargetValue}" /> class.
        /// </summary>
        public OperationCopyValueWithMapping(IConvertValue<TSourceValue, TTargetValue> convertValue)
        {
            convertValue.Should().NotBeNull();

            this.convertValue = convertValue;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            var sourceValue = this.sourceFunc.Invoke(source);
            var targetValue = this.convertValue.ConvertValue(sourceValue);
            target.SetPropertyValue(this.targetexpression, targetValue);
        }
    }
}
