// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IConvertHelper"/>.
    /// </summary>
    public class ConvertHelper : IConvertHelper
    {
        /// <summary>
        /// See <see cref="IConvertHelper.DoConvertPreProcessing{TSoureClass, TTargetClass}"/>.
        /// </summary>
        /// <typeparam name="TSourceClass">See link above.</typeparam>
        /// <typeparam name="TTargetClass">See link above.</typeparam>
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
                .ForEach(aX => aX.DoPreProcessing(source, target));
        }

        /// <summary>
        /// Start the convert post process it it's needed.
        /// </summary>
        /// <typeparam name="TSourceClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
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
                .ForEach(aX => aX.DoPostProcessing(source, target));
        }

        /// <summary>
        /// Evaluate the implementation of <see cref="IConvertInterception{TSoureClass,TTargetClass}"/>
        /// and return the result.
        /// </summary>
        /// <typeparam name="TSourceClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        public bool ContinueConvertProcess<TSourceClass, TTargetClass>(
            TSourceClass source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class
        {
            source.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lInterceptors = additionalProcessings.OfType<IConvertInterception<TSourceClass, TTargetClass>>();
            return !lInterceptors.Any() || lInterceptors.Any(aX => aX.CallConverter(source));
        }
    }
}