﻿namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class ConvertHelper : IConvertHelper
    {
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

            foreach (var addProcessing in additionalProcessings.OfType<IConvertPreProcessing<TSourceClass, TTargetClass>>())
            {
                addProcessing.DoPreProcessing(source, target);
            }
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

            foreach (var addProcessing in additionalProcessings.OfType<IConvertPostProcessing<TSourceClass, TTargetClass>>())
            {
                addProcessing.DoPostProcessing(source, target);
            }
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