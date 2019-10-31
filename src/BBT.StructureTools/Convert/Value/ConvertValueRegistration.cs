// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Value
{
    /// <summary>
    /// See <see cref="IConvertValueRegistration{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Type of the source.</typeparam>
    /// <typeparam name="TTarget">Type of the target.</typeparam>
    public class ConvertValueRegistration<TSource, TTarget> : IConvertValueRegistration<TSource, TTarget>
    {
        private readonly IValueConvertMapping<TSource, TTarget> valueConvertMapping = new ValueConvertMapping<TSource, TTarget>();

        /// <summary>
        /// See <see cref="IConvertValueRegistration{TSource, TTarget}.Register"/>.
        /// </summary>
        public IConvertValueRegistration<TSource, TTarget> Register(TSource sourceValue, TTarget targetValue)
        {
            this.valueConvertMapping.AddMapping(sourceValue, targetValue);
            return this;
        }

        /// <summary>
        /// See <see cref="IConvertValueRegistration{TSource, TTarget}.RegisterException"/>.
        /// </summary>
        public IConvertValueRegistration<TSource, TTarget> RegisterException(TSource sourceValue)
        {
            this.valueConvertMapping.AddException(sourceValue);
            return this;
        }

        /// <summary>
        /// See <see cref="IConvertValueRegistration{TSource, TTarget}.EndRegistrations"/>.
        /// </summary>
        public IValueConvertMapping<TSource, TTarget> EndRegistrations() => this.valueConvertMapping;
    }
}
