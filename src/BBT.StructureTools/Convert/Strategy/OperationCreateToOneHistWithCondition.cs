namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Constants;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCreateToOneHistWithCondition<TSource, TTarget, TSourceValue, TTargetValue, TReverseRelation, TTemporalData, TConvertIntention>
        : IOperationCreateToOneHistWithCondition<TSource, TTarget, TSourceValue, TTargetValue, TReverseRelation, TTemporalData, TConvertIntention>
        where TSource : class
        where TTarget : class, TReverseRelation
        where TSourceValue : class, TTemporalData
        where TTargetValue : class, TTemporalData
        where TTemporalData : class
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;
        private readonly ITemporalDataHandler<TTemporalData> targetValueTemporalDataHandler;
        private readonly ITemporalDataHandler<TTemporalData> sourceValueTemporalDataHandler;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TReverseRelation, TConvertIntention> createConvertHelper;
        private Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc;
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression;
        private Func<TSource, TTarget, bool> toOneHistCriteria;
        private Func<TSource, TTarget, DateTime> toOneReferenceDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCreateToOneHistWithCondition{TSource, TTarget, TSourceValue, TTargetValue, TReverseRelation, TTemporalData, TConvertIntention}" /> class.
        /// </summary>
        public OperationCreateToOneHistWithCondition(
            IConvertHelper convertHelper,
            ITemporalDataHandler<TTemporalData> targetValueTemporalDataHandler,
            ITemporalDataHandler<TTemporalData> sourceValueTemporalDataHandler)
        {
            convertHelper.NotNull(nameof(convertHelper));
            targetValueTemporalDataHandler.NotNull(nameof(targetValueTemporalDataHandler));

            this.convertHelper = convertHelper;
            this.targetValueTemporalDataHandler = targetValueTemporalDataHandler;
            this.sourceValueTemporalDataHandler = sourceValueTemporalDataHandler;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, bool> toOneHistCriteria,
            Func<TSource, TTarget, DateTime> toOneReferenceDate,
            ICreateConvertHelper<TSourceValue, TTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));
            toOneHistCriteria.NotNull(nameof(toOneHistCriteria));
            toOneReferenceDate.NotNull(nameof(toOneReferenceDate));
            createConvertHelper.NotNull(nameof(createConvertHelper));

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
            this.toOneHistCriteria = toOneHistCriteria;
            this.toOneReferenceDate = toOneReferenceDate;
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

            var copyToOne = this.toOneHistCriteria.Invoke(source, target);
            var toOneReferenceDate = this.toOneReferenceDate.Invoke(source, target);

            var sourceValues = this.sourceFunc.Invoke(source, target);

            var copies = new List<TTargetValue>();

            foreach (var sourceValue in sourceValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                {
                    continue;
                }

                if (copyToOne &&
                    !this.sourceValueTemporalDataHandler.IsReferenceDateWithin(sourceValue, toOneReferenceDate))
                {
                    continue;
                }

                var copy = this.createConvertHelper.CreateTarget(
                    sourceValue,
                    target,
                    additionalProcessings);
                copies.Add(copy);
            }

            if (copyToOne)
            {
                this.targetValueTemporalDataHandler.SetEndInfinte(copies[0], TemporalConstants.InfiniteDate);
            }

            target.AddRangeFilterNullValues(
                this.targetExpression,
                copies);
        }
    }
}
