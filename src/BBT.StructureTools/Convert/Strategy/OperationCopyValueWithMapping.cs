// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to copy a value of type <typeparamref name="TSourceValue"/> into
    /// property of type <typeparamref name="TTargetValue"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    /// <typeparam name="TSourceValue">The type of the value to copy.</typeparam>
    /// <typeparam name="TTargetValue">The type of the value to copy.</typeparam>
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

        /// <summary>
        /// See <see cref="IOperationCopyValueWithMapping{TSource,TTarget,TSourceValue, TTargetValue}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
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

            var sourceValue = this.sourceFunc.Invoke(source);
            var targetValue = this.convertValue.ConvertValue(sourceValue);
            target.SetPropertyValue(this.targetexpression, targetValue);
        }
    }
}
