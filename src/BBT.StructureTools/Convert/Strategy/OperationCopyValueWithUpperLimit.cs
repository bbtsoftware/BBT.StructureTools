namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationCopyValueWithUpperLimit{TSource,TTarget,TValue}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TValue">See link above.</typeparam>
    public class OperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        : IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
        where TValue : IComparable<TValue>
    {
        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> mSourceFunc;

        /// <summary>
        /// Function to get the look-up value.
        /// </summary>
        private Func<TSource, TValue> mSourceUpperLimitFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> mTargetExpression;

        /// <summary>
        /// See <see cref="IOperationCopyValueWithUpperLimit{TSource,TTarget,TValue}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Func<TSource, TValue> aSourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.Should().NotBeNull();
            aSourceUpperLimitFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

            this.mSourceFunc = aSourceFunc;
            this.mSourceUpperLimitFunc = aSourceUpperLimitFunc;
            this.mTargetExpression = aTargetExpression;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource aSource,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> aAdditionalProcessings)
        {
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();

            var lSourceValue = this.mSourceFunc.Invoke(aSource);
            var lUpperLimitValue = this.mSourceUpperLimitFunc(aSource);

            var lValue = LookupUtils.ApplyUpperLimit(lSourceValue, lUpperLimitValue);
            aTarget.SetPropertyValue(
                this.mTargetExpression,
                lValue);
        }
    }
}
