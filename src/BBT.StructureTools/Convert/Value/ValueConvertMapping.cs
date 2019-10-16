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
        private readonly Dictionary<TSource, TTarget> mMapping = new Dictionary<TSource, TTarget>();
        private readonly HashSet<TSource> mMapToException = new HashSet<TSource>();
        private Func<TTarget> mNullCaseFunc;

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.AddException"/>.
        /// </summary>
        public void AddException(TSource sourceValue)
        {
            this.CheckNotAlreadyRegistered(sourceValue);
            this.mMapToException.Add(sourceValue);
        }

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.AddMapping"/>.
        /// </summary>
        public void AddMapping(TSource sourceValue, TTarget targetValue)
        {
            this.CheckNotAlreadyRegistered(sourceValue);

            if (sourceValue == null)
            {
                this.mNullCaseFunc = () => targetValue;
                return;
            }

            this.mMapping.Add(sourceValue, targetValue);
        }

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.IsRegisteredForException"/>.
        /// </summary>
        public bool IsRegisteredForException(TSource sourceValue) => this.mMapToException.Contains(sourceValue);

        /// <summary>
        /// See <see cref="IValueConvertMapping{TSource, TTarget}.TryGetValue"/>.
        /// </summary>
        public bool TryGetValue(TSource sourceValue, out TTarget targetValue)
        {
            if (sourceValue == null)
            {
                if (this.mNullCaseFunc == null)
                {
                    targetValue = default(TTarget);
                    return false;
                }

                targetValue = this.mNullCaseFunc();
                return true;
            }

            return this.mMapping.TryGetValue(sourceValue, out targetValue);
        }

        /// <summary>
        /// Throw an exception if <paramref name="sourceValue"/> is already registered.
        /// </summary>
        private void CheckNotAlreadyRegistered(TSource sourceValue)
        {
            if (sourceValue == null)
            {
                if (this.mNullCaseFunc != null)
                {
                    throw new CopyConvertCompareException(
                        FormattableString.Invariant($"Null case is already registered for target value {this.mNullCaseFunc()}."));
                }

                return;
            }

            if (this.mMapping.TryGetValue(sourceValue, out var lTargetValue))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"{sourceValue} is already registered for target value {lTargetValue}."));
            }

            if (this.mMapToException.Contains(sourceValue))
            {
                throw new CopyConvertCompareException(FormattableString.Invariant($"{sourceValue} is already registered for an exception."));
            }
        }
    }
}
