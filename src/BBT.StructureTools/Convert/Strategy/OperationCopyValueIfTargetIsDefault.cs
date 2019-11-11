namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        : IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private Func<TSource, TValue> sourceFunc;
        private Expression<Func<TTarget, TValue>> targetexpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.NotNull(nameof(aSourceFunc));
            aTargetExpression.NotNull(nameof(aTargetExpression));

            this.sourceFunc = aSourceFunc;
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
            var targetValue = aTarget.GetPropertyValue(this.targetexpression);
            if (!LookupUtils.IsDefaultValue(targetValue))
            {
                return;
            }

            aTarget.SetPropertyValue(
                this.targetexpression,
                sourceValue);
        }
    }
}
