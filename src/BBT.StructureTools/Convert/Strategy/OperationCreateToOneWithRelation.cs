namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCreateToOneWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TRelation, TConvertIntention>
        : IOperationCreateToOneWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TTargetValue>> targetExpression;

        private Func<TSource, TTarget, TRelation> relationFunc;
        private ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper)
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

            var sourceValue = this.sourceFunc.Invoke(source);

            if (sourceValue == null)
            {
                return;
            }

            var relation = this.relationFunc(source, target);

            var targetValue = this.createConvertHelper.CreateTarget(
                    sourceValue,
                    relation,
                    additionalProcessings);

            target.SetPropertyValue(
                this.targetExpression,
                targetValue);
        }
    }
}
