namespace BBT.StructureTools.Convert
{
    /// <inheritdoc/>
    internal class ConvertEngine<TSource, TTarget> : IConvertEngine<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertEngine{TSource, TTarget}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public ConvertEngine()
        {
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> StartRegistrations()
        {
            return new ConvertRegistration<TSource, TTarget>();
        }
    }
}
