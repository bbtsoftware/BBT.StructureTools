namespace BBT.StructureTools.Convert
{
    /// <inheritdoc/>
    internal class ConvertEngine<TSource, TTarget> : IConvertEngine<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> StartRegistrations()
        {
            return new ConvertRegistration<TSource, TTarget>();
        }
    }
}
