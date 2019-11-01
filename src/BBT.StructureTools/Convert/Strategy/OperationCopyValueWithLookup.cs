﻿namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationCopyValueWithLookUp<TSource, TTarget, TValue>
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
            aSourceFunc.Should().NotBeNull();
            aSourceLookUpFunc.Should().NotBeNull();
            aTargetExpression.Should().NotBeNull();

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
            aSource.Should().NotBeNull();
            aTarget.Should().NotBeNull();

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
