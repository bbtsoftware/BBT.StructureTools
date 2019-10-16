// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Value
{
    /// <summary>
    /// Mapping for values of type <typeparamref name="TSource"/> to values of type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public interface IValueConvertMapping<TSource, TTarget>
    {
        /// <summary>
        /// Add the value pair (<paramref name="sourceValue"/>, <paramref name="targetValue"/>) to the mapping.
        /// </summary>
        void AddMapping(TSource sourceValue, TTarget targetValue);

        /// <summary>
        /// Register an exception for a <paramref name="sourceValue"/>, i.e. there is no corresponding value in the target type.
        /// </summary>
        void AddException(TSource sourceValue);

        /// <summary>
        /// Return the target value for <paramref name="sourceValue"/> and true,
        /// or false if the map contains no target value registration for source value.
        /// </summary>
        bool TryGetValue(TSource sourceValue, out TTarget targetValue);

        /// <summary>
        /// Returns true if <paramref name="sourceValue"/> is registered for an exception.
        /// </summary>
        bool IsRegisteredForException(TSource sourceValue);
    }
}
