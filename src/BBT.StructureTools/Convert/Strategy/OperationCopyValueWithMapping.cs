namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>
        : IOperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>
        where TSource : class
        where TTarget : class
    {
        private readonly IConvertValue<TSourceValue, TTargetValue> convertValue;
        private Func<TSource, TSourceValue> sourceFunc;
        private Expression<Func<TTarget, TTargetValue>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithMapping{TSource, TTarget, TSourceValue, TTargetValue}" /> class.
        /// </summary>
        public OperationCopyValueWithMapping(IConvertValue<TSourceValue, TTargetValue> convertValue)
        {
            StructureToolsArgumentChecks.NotNull(convertValue, nameof(convertValue));
            this.convertValue = convertValue;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));

            var sourceValue = this.sourceFunc.Invoke(source);
            var targetValue = this.convertValue.ConvertValue(sourceValue);
            target.SetPropertyValue(this.targetExpression, targetValue);
        }
    }
}
