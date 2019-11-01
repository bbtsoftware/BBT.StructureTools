namespace BBT.StructureTools.Convert.Value
{
    /// <inheritdoc/>
    public class ConvertValueRegistration<TSource, TTarget> : IConvertValueRegistration<TSource, TTarget>
    {
        private readonly IValueConvertMapping<TSource, TTarget> valueConvertMapping = new ValueConvertMapping<TSource, TTarget>();

        /// <inheritdoc/>
        public IConvertValueRegistration<TSource, TTarget> Register(TSource sourceValue, TTarget targetValue)
        {
            this.valueConvertMapping.AddMapping(sourceValue, targetValue);
            return this;
        }

        /// <inheritdoc/>
        public IConvertValueRegistration<TSource, TTarget> RegisterException(TSource sourceValue)
        {
            this.valueConvertMapping.AddException(sourceValue);
            return this;
        }

        /// <inheritdoc/>
        public IValueConvertMapping<TSource, TTarget> EndRegistrations() => this.valueConvertMapping;
    }
}
