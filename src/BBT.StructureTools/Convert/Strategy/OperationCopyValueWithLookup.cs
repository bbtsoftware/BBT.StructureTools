namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationCopyValueWithLookUp{TSource,TTarget,TValue}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TValue">See link above.</typeparam>
    public class OperationCopyValueWithLookUp<TSource, TTarget, TValue>
        : IOperationCopyValueWithLookUp<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> mSourceFunc;

        /// <summary>
        /// Function to get the look-up value.
        /// </summary>
        private Func<TSource, TValue> mSourceLookUpFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> mTargetExpression;

        /// <summary>
        /// See <see cref="IOperationCopyValueWithLookUp{TSource,TTarget,TValue}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, TValue> aSourceFunc,
            Func<TSource, TValue> aSourceLookUpFunc,
            Expression<Func<TTarget, TValue>> aTargetExpression)
        {
            aSourceFunc.Should().NotBeNull();
            aSourceLookUpFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

            this.mSourceFunc = aSourceFunc;
            this.mSourceLookUpFunc = aSourceLookUpFunc;
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

            if (LookupUtils.IsDefaultValue(lSourceValue))
            {
                lSourceValue = this.mSourceLookUpFunc.Invoke(aSource);
            }

            aTarget.SetPropertyValue(
                this.mTargetExpression,
                lSourceValue);
        }
    }
}
