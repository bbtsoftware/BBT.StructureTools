namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    /// <inheritdoc/>
    public class ConvertHelper : IConvertHelper
    {
        /// <inheritdoc/>
        public void DoConvertPreProcessing<TSourceClass, TTargetClass>(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            additionalProcessings
                .OfType<IConvertPreProcessing<TSourceClass, TTargetClass>>()
                .ToList()
                .ForEach(x => x.DoPreProcessing(source, target));
        }

        /// <inheritdoc/>
        public void DoConvertPostProcessing<TSourceClass, TTargetClass>(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            additionalProcessings
                .OfType<IConvertPostProcessing<TSourceClass, TTargetClass>>()
                .ToList()
                .ForEach(x => x.DoPostProcessing(source, target));
        }

        /// <inheritdoc/>
        public bool ContinueConvertProcess<TSourceClass, TTargetClass>(
            TSourceClass source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class
        {
            source.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var interceptors = additionalProcessings.OfType<IConvertInterception<TSourceClass, TTargetClass>>();
            return !interceptors.Any() || interceptors.Any(x => x.CallConverter(source));
        }
    }
}