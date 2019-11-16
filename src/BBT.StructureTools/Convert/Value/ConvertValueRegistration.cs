namespace BBT.StructureTools.Convert.Value
{
    /// <inheritdoc/>
    internal class ConvertValueRegistration<TSource, TTarget> : IConvertValueRegistration<TSource, TTarget>
    {
        private readonly IValueConvertMapping<TSource, TTarget> valueConvertMapping = new ValueConvertMapping<TSource, TTarget>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertValueRegistration{TSource, TTarget}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public ConvertValueRegistration()
        {
        }

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
