namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCreateToOneWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>
        : IOperationCreateToOneWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
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
        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper;

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(relationFunc, nameof(relationFunc));
            StructureToolsArgumentChecks.NotNull(createConvertHelper, nameof(createConvertHelper));

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
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

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
