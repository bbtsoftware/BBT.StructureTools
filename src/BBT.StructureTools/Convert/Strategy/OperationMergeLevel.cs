namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
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
        private Func<TSource, IEnumerable<TMergeValue>> mergeFunc;
        private Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc;
        private Func<TTarget, ICollection<TTargetValue>> targetFunc;
        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMergeLevel{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TMergeValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationMergeLevel(
            IConvertHelper convertHelper)
        {
            convertHelper.NotNull(nameof(convertHelper));

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TMergeValue>> aMergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
        {
            aMergeFunc.NotNull(nameof(aMergeFunc));
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));
            createConvertHelper.NotNull(nameof(createConvertHelper));

            this.mergeFunc = aMergeFunc;
            this.sourceFunc = sourceFunc;
            this.targetFunc = targetExpression.Compile();
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

                    if (copy != null)
                    {
                        copies.Add(copy);
                    }
                }
            }

            var targetCollection = this.targetFunc.Invoke(target);
            targetCollection.AddRangeToMe(copies);
        }
    }
}
