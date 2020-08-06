namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>
        : IOperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TTargetValue>> targetExpression;

        /// <inheritdoc/>
        public void Initialize(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(createConvertHelper, nameof(createConvertHelper));

            this.targetExpression = targetExpression;
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

            if (source == null)
            {
                return;
            }

            var targetValue = this.createConvertHelper.CreateTarget(
                    source,
                    target,
                    additionalProcessings);

            target.SetPropertyValue(
                this.targetExpression,
                targetValue);
        }
    }
}
