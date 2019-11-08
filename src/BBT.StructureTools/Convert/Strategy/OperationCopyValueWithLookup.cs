namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyValueWithLookUp<TSource, TTarget, TValue>
        : IOperationCopyValueWithLookUp<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private Func<TSource, TValue> sourceFunc;
        private Func<TSource, TValue> sourceLookupFunc;
        private Expression<Func<TTarget, TValue>> targetexpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Func<TSource, TValue> aSourceLookUpFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.NotNull(nameof(aSourceFunc));
            aSourceLookUpFunc.NotNull(nameof(aSourceLookUpFunc));
            aTargetExpression.NotNull(nameof(aTargetExpression));

            this.sourceFunc = aSourceFunc;
            this.sourceLookupFunc = aSourceLookUpFunc;
            this.targetexpression = aTargetExpression;
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

            if (LookupUtils.IsDefaultValue(sourceValue))
            {
                sourceValue = this.sourceLookupFunc.Invoke(aSource);
            }

            aTarget.SetPropertyValue(
                this.targetexpression,
                sourceValue);
        }
    }
}
