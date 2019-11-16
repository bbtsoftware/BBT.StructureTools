namespace BBT.StructureTools.Convert.Value
{
    using System;
    using System.Collections.Generic;

    /// <inheritdoc/>
    internal class ValueConvertMapping<TSource, TTarget> : IValueConvertMapping<TSource, TTarget>
    {
        private readonly Dictionary<TSource, TTarget> mapping = new Dictionary<TSource, TTarget>();
        private readonly HashSet<TSource> mapToException = new HashSet<TSource>();
        private Func<TTarget> nullCaseFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConvertMapping{TSource, TTarget}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public ValueConvertMapping()
        {
        }

        /// <inheritdoc/>
        public void AddException(TSource sourceValue)
        {
            this.CheckNotAlreadyRegistered(sourceValue);
            this.mapToException.Add(sourceValue);
        }

        /// <inheritdoc/>
        public void AddMapping(TSource sourceValue, TTarget targetValue)
        {
            this.CheckNotAlreadyRegistered(sourceValue);

            if (sourceValue == null)
            {
                this.nullCaseFunc = () => targetValue;
                return;
            }

            this.mapping.Add(sourceValue, targetValue);
        }

        /// <inheritdoc/>
        public bool IsRegisteredForException(TSource sourceValue) => this.mapToException.Contains(sourceValue);

        /// <inheritdoc/>
        public bool TryGetValue(TSource sourceValue, out TTarget targetValue)
        {
            if (sourceValue == null)
            {
                if (this.nullCaseFunc == null)
                {
                    targetValue = default;
                    return false;
                }

                targetValue = this.nullCaseFunc();
                return true;
            }

            return this.mapping.TryGetValue(sourceValue, out targetValue);
        }

        /// <summary>
        /// Throw an exception if <paramref name="sourceValue"/> is already registered.
        /// </summary>
        private void CheckNotAlreadyRegistered(TSource sourceValue)
        {
            if (sourceValue == null)
            {
                if (this.nullCaseFunc != null)
                {
                    throw new CopyConvertCompareException(
                        FormattableString.Invariant($"Null case is already registered for target value {this.nullCaseFunc()}."));
                }

                return;
            }

            if (this.mapping.TryGetValue(sourceValue, out var targetValue))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"{sourceValue} is already registered for target value {targetValue}."));
            }

            if (this.mapToException.Contains(sourceValue))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"{sourceValue} is already registered for an exception."));
            }
        }
    }
}
