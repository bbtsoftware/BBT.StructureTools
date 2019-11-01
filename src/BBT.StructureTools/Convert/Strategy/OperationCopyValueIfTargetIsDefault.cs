namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
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
            aSourceFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

            this.sourceFunc = aSourceFunc;
            this.targetexpression = aTargetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();

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
