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
        private readonly IConvertValue<TSourceValue, TTargetValue> mConvertValue;
        private Func<TSource, TSourceValue> mSourceFunc;
        private Expression<Func<TTarget, TTargetValue>> mTargetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithMapping{TSource, TTarget, TSourceValue, TTargetValue}" /> class.
        /// </summary>
        public OperationCopyValueWithMapping(IConvertValue<TSourceValue, TTargetValue> aConvertValue)
        {
            aConvertValue.Should().NotBeNull();

            this.mConvertValue = aConvertValue;
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

            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
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

            var lSourceValue = this.mSourceFunc.Invoke(source);
            var lTargetValue = this.mConvertValue.ConvertValue(lSourceValue);
            target.SetPropertyValue(this.mTargetExpression, lTargetValue);
        }
    }
}
