﻿// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConcreteTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationCreateToManyGeneric<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IOperationCreateToManyGeneric<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper mConvertHelper;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> mCreateConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> mSourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, IEnumerable<TTargetValue>>> mTargetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCreateToManyGeneric{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationCreateToManyGeneric(
            IConvertHelper aConvertHelper)
        {
            aConvertHelper.Should().NotBeNull();

            this.mConvertHelper = aConvertHelper;
        }

        /// <summary>
        /// See <see cref="IOperationCreateToManyGeneric{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TConvertIntention}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> aCreateConvertHelper)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            aCreateConvertHelper.Should().NotBeNull();

            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
            this.mCreateConvertHelper = aCreateConvertHelper;
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
            additionalProcessings.Should().NotBeNull();

            var lSourceValues = this.mSourceFunc.Invoke(source);

            var lCopies = new List<TTargetValue>();

            foreach (var lSourceValue in lSourceValues)
            {
                if (!this.mConvertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    lSourceValue, additionalProcessings))
                {
                    continue;
                }

                var lCopy = this.mCreateConvertHelper.CreateTarget(
                    lSourceValue,
                    additionalProcessings);
                lCopies.Add(lCopy);
            }

            target.SetPropertyValue(this.mTargetExpression, lCopies);
        }
    }
}
