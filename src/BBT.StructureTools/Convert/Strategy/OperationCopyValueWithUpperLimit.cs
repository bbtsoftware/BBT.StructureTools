namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        : IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
        where TValue : IComparable<TValue>
    {
        private Func<TSource, TValue> sourceFunc;
        private Func<TSource, TValue> sourceUpperLimitFunc;
        private Expression<Func<TTarget, TValue>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithUpperLimit{TSource, TTarget, TValue}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public OperationCopyValueWithUpperLimit()
        {
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Func<TSource, TValue> aSourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.NotNull(nameof(aSourceFunc));
            aSourceUpperLimitFunc.NotNull(nameof(aSourceUpperLimitFunc));
            aTargetExpression.NotNull(nameof(aTargetExpression));

            this.sourceFunc = aSourceFunc;
            this.sourceUpperLimitFunc = aSourceUpperLimitFunc;
            this.targetExpression = aTargetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            aSource.NotNull(nameof(aSource));
            aTarget.NotNull(nameof(aTarget));

            var sourceValue = this.sourceFunc.Invoke(aSource);
            var upperLimitValue = this.sourceUpperLimitFunc(aSource);

            var value = LookupUtils.ApplyUpperLimit(sourceValue, upperLimitValue);
            aTarget.SetPropertyValue(
                this.targetExpression,
                value);
        }
    }
}
