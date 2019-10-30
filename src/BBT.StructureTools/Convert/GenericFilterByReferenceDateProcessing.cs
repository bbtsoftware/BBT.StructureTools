// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;

    /// <summary>
    /// Provides a filter for a specific reference date.
    /// Generic implementation of <see cref="IConvertInterception{TSoureClass,TTargetClass}"/>.
    /// </summary>
    /// <typeparam name="TSourceClass">See link above.</typeparam>
    /// <typeparam name="TTargetClass">Also see link above.</typeparam>
    public class GenericFilterByReferenceDateProcessing<TSourceClass, TTargetClass> : GenericContinueConvertInterception<TSourceClass, TTargetClass>
        where TSourceClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFilterByReferenceDateProcessing{TSourceClass, TTargetClass}" /> class.
        /// </summary>
        public GenericFilterByReferenceDateProcessing(
            DateTime aReferenceDate,
            ITemporalDataDescriptor<TSourceClass> aTemporalSourceDataDescriptor)
            : base(
            aX => aTemporalSourceDataDescriptor.GetBegin(aX) <= aReferenceDate && aTemporalSourceDataDescriptor.GetEnd(aX) >= aReferenceDate)
        {
        }
    }
}
