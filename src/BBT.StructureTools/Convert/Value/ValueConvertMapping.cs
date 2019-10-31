// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Value
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the <see cref="IValueConvertMapping{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public class ValueConvertMapping<TSource, TTarget> : IValueConvertMapping<TSource, TTarget>
    {
        private readonly Dictionary<TSource, TTarget> mapping = new Dictionary<TSource, TTarget>();
        private readonly HashSet<TSource> mapToException = new HashSet<TSource>();
        private Func<TTarget> nullCaseFunc;

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.AddException"/>.
        /// </summary>
        public void AddException(TSource sourceValue)
        {
            this.CheckNotAlreadyRegistered(sourceValue);
            this.mapToException.Add(sourceValue);
        }

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.AddMapping"/>.
        /// </summary>
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

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.IsRegisteredForException"/>.
        /// </summary>
        public bool IsRegisteredForException(TSource sourceValue) => this.mapToException.Contains(sourceValue);

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.TryGetValue"/>.
        /// </summary>
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
