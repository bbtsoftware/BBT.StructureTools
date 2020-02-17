namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class ConvertHelper : IConvertHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertHelper"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public ConvertHelper()
        {
        }

        /// <inheritdoc/>
        public void DoConvertPreProcessing<TSourceClass, TTargetClass>(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

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
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

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
            source.NotNull(nameof(source));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var interceptors = additionalProcessings.OfType<IConvertInterception<TSourceClass, TTargetClass>>();
            return !interceptors.Any() || interceptors.Any(x => x.CallConverter(source));
        }
    }
}