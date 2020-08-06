namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>
        : IOperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TMergeValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, IEnumerable<TMergeValue>> mergeFunc;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMergeLevel{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TMergeValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationMergeLevel(
            IConvertHelper convertHelper)
        {
            StructureToolsArgumentChecks.NotNull(convertHelper, nameof(convertHelper));

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TMergeValue>> mergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            StructureToolsArgumentChecks.NotNull(mergeFunc, nameof(mergeFunc));
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(createConvertHelper, nameof(createConvertHelper));

            this.mergeFunc = mergeFunc;
            this.sourceFunc = sourceFunc;
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
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var mergeValues = this.mergeFunc.Invoke(source);

            var copies = new List<TTargetValue>();

            foreach (var mergeValue in mergeValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TMergeValue, TTargetValue>(
                    mergeValue, additionalProcessings))
                {
                    continue;
                }

                var sourceValues = this.sourceFunc.Invoke(mergeValue);

                foreach (var sourceValue in sourceValues)
                {
                    if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                    {
                        continue;
                    }

                    var copy = this.createConvertHelper.CreateTarget(
                        sourceValue,
                        target,
                        additionalProcessings);
                    copies.Add(copy);
                }
            }

            target.AddRangeFilterNullValues(
                this.targetExpression,
                copies);
        }
    }
}
