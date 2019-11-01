namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using FluentAssertions;

    /// <inheritdoc/>
    public class Converter<TSource, TTarget, TConvertIntention> : IConvert<TSource, TTarget, TConvertIntention>
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
            convertEngine.Should().NotBeNull();
            convertRegistrations.Should().NotBeNull();
            convertHelper.Should().NotBeNull();

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
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            this.convertHelper.DoConvertPreProcessing(source, target, additionalProcessings);
            this.convertOperations.Convert(source, target, additionalProcessings);
            this.convertHelper.DoConvertPostProcessing(source, target, additionalProcessings);
        }
    }
}
