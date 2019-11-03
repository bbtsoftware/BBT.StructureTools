namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class OperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        : IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
        where TValue : IComparable<TValue>
    {
        private Func<TSource, TValue> sourceFunc;
        private Func<TSource, TValue> sourceUpperLimitFunc;
        private Expression<Func<TTarget, TValue>> targetexpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Func<TSource, TValue> aSourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.Should().NotBeNull();
            aSourceUpperLimitFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

            this.sourceFunc = aSourceFunc;
            this.sourceUpperLimitFunc = aSourceUpperLimitFunc;
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
            var upperLimitValue = this.sourceUpperLimitFunc(aSource);

            var value = LookupUtils.ApplyUpperLimit(sourceValue, upperLimitValue);
            aTarget.SetPropertyValue(
                this.targetexpression,
                value);
        }
    }
}
