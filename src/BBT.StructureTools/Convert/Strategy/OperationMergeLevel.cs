namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TMergeValue, TConvertIntention>
        : IOperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TMergeValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
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

        private ICreateConvertHelper<TSourceValue, TTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMergeLevel{TSource,TTarget,TSourceValue,TTargetValue,TMergeValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationMergeLevel(
            IConvertHelper convertHelper)
        {
            convertHelper.NotNull(nameof(convertHelper));

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TMergeValue>> mergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            mergeFunc.NotNull(nameof(mergeFunc));
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));
            createConvertHelper.NotNull(nameof(createConvertHelper));

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
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var mergeValues = this.mergeFunc.Invoke(source);

            foreach (var mergeValue in mergeValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TMergeValue, TTargetValue>(
                    mergeValue, additionalProcessings))
                {
                    continue;
                }

                var sourceValues = this.sourceFunc.Invoke(mergeValue);

                var targetList = target.GetList(this.targetExpression);
                foreach (var sourceValue in sourceValues)
                {
                    if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                    {
                        continue;
                    }

                    var copy = this.createConvertHelper.Create(
                        sourceValue,
                        target);

                    targetList.AddUnique(copy);

                    this.createConvertHelper.Convert(
                        sourceValue,
                        copy,
                        additionalProcessings);
                }
            }
        }
    }
}
