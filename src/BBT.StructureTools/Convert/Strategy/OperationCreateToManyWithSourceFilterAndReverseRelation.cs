namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCreateToManyWithSourceFilterAndReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>
        : IOperationCreateToManyWithSourceFilterAndReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class, TReverseRelation
        where TSourceValue : class
        where TTargetValue : class
        where TConcreteTargetValue : TTargetValue, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertHelper convertHelper;

        private ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCreateToManyWithSourceFilterAndReverseRelation{TSource,TTarget,TSourceValue,TTargetValue,TConcreteTargetValue,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public OperationCreateToManyWithSourceFilterAndReverseRelation(
            IConvertHelper convertHelper)
        {
            StructureToolsArgumentChecks.NotNull(convertHelper, nameof(convertHelper));

            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(createConvertHelper, nameof(createConvertHelper));

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

            var sourceValues = this.sourceFunc.Invoke(source, target);

            var copies = new List<TTargetValue>();

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

            target.AddRangeFilterNullValues(
                this.targetExpression,
                copies);
        }
    }
}
