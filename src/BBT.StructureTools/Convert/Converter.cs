namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class Converter<TSource, TTarget, TConvertIntention> : IConvert<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvertOperations<TSource, TTarget> convertOperations;
        private readonly IConvertHelper convertHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter{TSource, TTarget, TConvertIntention}" /> class.
        /// </summary>
        public Converter(
            IConvertRegistrations<TSource, TTarget, TConvertIntention> convertRegistrations,
            IConvertEngine<TSource, TTarget> convertEngine,
            IConvertHelper convertHelper)
        {
            StructureToolsArgumentChecks.NotNull(convertEngine, nameof(convertEngine));
            StructureToolsArgumentChecks.NotNull(convertRegistrations, nameof(convertRegistrations));
            StructureToolsArgumentChecks.NotNull(convertHelper, nameof(convertHelper));

            this.convertHelper = convertHelper;
            var registrations = convertEngine.StartRegistrations();
            convertRegistrations.DoRegistrations(registrations);
            this.convertOperations = registrations.EndRegistrations();
        }

        /// <inheritdoc/>
        public void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            this.convertHelper.DoConvertPreProcessing(source, target, additionalProcessings);
            this.convertOperations.Convert(source, target, additionalProcessings);
            this.convertHelper.DoConvertPostProcessing(
                source, target, additionalProcessings);
        }
    }
}
