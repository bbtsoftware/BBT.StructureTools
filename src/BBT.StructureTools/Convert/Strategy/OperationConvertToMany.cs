namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> convert;
        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// The function to obtain the source values.
        /// </summary>
        private Func<TSource, IEnumerable<TSourceValue>> sourceFunc;

        /// <summary>
        /// The function to obtain the target values.
        /// </summary>
        private Func<TTarget, IEnumerable<TTargetValue>> targetFunc;

        /// <summary>
        /// The function to filter the target value corresponding to the source value.
        /// </summary>
        private Func<TSourceValue, TTargetValue, bool> filterFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertToMany{TSource,TTarget,TSourceValue,TTargetValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertToMany(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> convert,
            IConvertHelper convertHelper)
        {
            StructureToolsArgumentChecks.NotNull(convert, nameof(convert));
            StructureToolsArgumentChecks.NotNull(convertHelper, nameof(convertHelper));

            this.convert = convert;
            this.convertHelper = convertHelper;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> filterFunc)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetFunc, nameof(targetFunc));
            StructureToolsArgumentChecks.NotNull(filterFunc, nameof(filterFunc));

            this.sourceFunc = sourceFunc;
            this.targetFunc = targetFunc;
            this.filterFunc = filterFunc;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));

            var sourceValues = this.sourceFunc(source).ToList();
            var targetValues = this.targetFunc(target).Where(x => x != null).ToList();

            foreach (var sourceValue in sourceValues)
            {
                if (!this.convertHelper.ContinueConvertProcess<TSourceValue, TTargetValue>(
                    sourceValue, additionalProcessings))
                {
                    continue;
                }

                var errorMsg = string.Format(CultureInfo.InvariantCulture, $"One result expected to convert from '{typeof(TSourceValue).Name}' to '{typeof(TTargetValue).Name}'");
                var targetValue = targetValues
                    .SingleWithExceptionMessage(x => this.filterFunc(sourceValue, x), errorMsg);
                this.convert.Convert(sourceValue, targetValue, additionalProcessings);
            }
        }
    }
}
