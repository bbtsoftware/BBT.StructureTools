namespace BBT.StructureTools.Convert
{
    using System;

    /// <summary>
    /// Provides a filter for a specific reference date.
    /// Generic implementation of <see cref="IConvertInterception{TSourceClass,TTargetClass}"/>.
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
            DateTime referenceDate,
            ITemporalDataDescriptor<TSourceClass> temporalSourceDataDescriptor)
            : base(
            x => temporalSourceDataDescriptor.GetBegin(x) <= referenceDate && temporalSourceDataDescriptor.GetEnd(x) >= referenceDate)
        {
        }
    }
}
