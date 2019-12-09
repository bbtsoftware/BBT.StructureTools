namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> convert;

        private Func<TSource, TSourceValue> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromSourceOnDifferentLevels{TSource,TTarget,TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromSourceOnDifferentLevels(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> convert)
        {
            convert.NotNull(nameof(convert));

            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TSourceValue> sourceFunc)
        {
            sourceFunc.NotNull(nameof(sourceFunc));

            this.sourceFunc = sourceFunc;
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
            target.IsOfType<TTargetValue>(nameof(target));

            // Need to use another collection, since the given collection my change and the enumeration fails.
            var newAdditionalProcessings = new List<IBaseAdditionalProcessing>(additionalProcessings);

            if (this.sourceFunc(source) == null)
            {
                return;
            }

            additionalProcessings.Add(
                new GenericConvertPostProcessing<TSource, TTarget>(
                    (subSource, subTarget) =>
                    {
                        // Once added post processings are executed on each model tree.
                        // Therefore this additional null guard is needed.
                        if (this.sourceFunc(subSource) == null)
                        {
                            return;
                        }

                        this.convert.Convert(this.sourceFunc(subSource), subTarget as TTargetValue, newAdditionalProcessings);
                    }));
        }
    }
}
