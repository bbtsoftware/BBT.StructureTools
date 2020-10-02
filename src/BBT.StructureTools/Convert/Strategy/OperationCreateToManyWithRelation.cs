﻿namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCreateToManyWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>
        : IOperationCreateToManyWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression;

        private Func<TSource, TTarget, TRelation> relationFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCreateToManyWithRelation{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TRelation,TConvertIntention}" /> class.
        /// </summary>
        public OperationCreateToManyWithRelation(
            IConvertHelper convertHelper)
        {
            convertHelper.NotNull(nameof(convertHelper));

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));
            relationFunc.NotNull(nameof(relationFunc));
            createConvertHelper.NotNull(nameof(createConvertHelper));

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
            this.relationFunc = relationFunc;
            this.createConvertHelper = createConvertHelper;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var sourceValues = this.sourceFunc.Invoke(source);

            var copies = new List<TTargetValue>();

            var relation = this.relationFunc(source, target);

            foreach (var sourceValue in sourceValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                {
                    continue;
                }

                var copy = this.createConvertHelper.CreateTarget(
                    sourceValue,
                    relation,
                    additionalProcessings);
                copies.Add(copy);
            }

            target.AddRangeFilterNullValues(
                this.targetExpression,
                copies);
        }
    }
}
