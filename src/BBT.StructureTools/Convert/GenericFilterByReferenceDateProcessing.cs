namespace BBT.StructureTools.Convert
{
    using BBT.StructureTools.Provider;

    /// <summary>
    /// Provides a filter for a specific reference date.
    /// Generic implementation of <see cref="IConvertInterception{TSoureClass,TTargetClass}"/>.
    /// </summary>
    /// <typeparam name="TSourceClass">Source type (also <typeparamref name="TTemporalData"/>).</typeparam>
    /// <typeparam name="TTargetClass">Target type.</typeparam>
    /// <typeparam name="TTemporalData">Temporal data type used within the <see cref="ITemporalDataHandler{TTemporalData}"/>.</typeparam>
    internal class GenericFilterByReferenceDateProcessing<TSourceClass, TTargetClass, TTemporalData> : GenericContinueConvertInterception<TSourceClass, TTargetClass>
        where TSourceClass : class, TTemporalData
        where TTargetClass : class
        where TTemporalData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFilterByReferenceDateProcessing{TSourceClass, TTargetClass, TTemporalData}" /> class.
        /// </summary>
        /// <param name="temporalDataHandler">
        /// The <see cref="ITemporalDataHandler{T}"/> implementation which is used
        /// to determine whether the <paramref name="referenceDate"/> is within temporal data's (passed in later on)
        /// time range.
        /// </param>
        /// <param name="referenceDate">
        /// All <typeparamref name="TSourceClass"/> history sections are filtered by <paramref name="referenceDate"/>.
        /// All history sections not containing <paramref name="referenceDate"/> are skipped.
        /// </param>
        public GenericFilterByReferenceDateProcessing(ITemporalDataHandler<TTemporalData> temporalDataHandler, System.DateTime referenceDate)
            : base(x => temporalDataHandler.IsReferenceDateWithin(x, referenceDate))
        {
        }
    }
}
