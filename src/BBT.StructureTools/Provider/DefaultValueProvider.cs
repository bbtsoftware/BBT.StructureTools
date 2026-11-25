namespace BBT.StructureTools.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc/>
    internal class DefaultValueProvider : IDefaultValueProvider
    {
        private readonly Dictionary<Type, List<object>> defaultValuesDic;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueProvider"/> class.
        /// </summary>
        public DefaultValueProvider()
        {
            this.defaultValuesDic = new Dictionary<Type, List<object>>();
        }

        /// <inheritdoc/>
        public T ApplyUpperLimit<T>(T specificValue, T upperLimitValue)
            where T : IComparable<T>
        {
            if (this.GetDefaultValues<T>().Contains(upperLimitValue))
            {
                return specificValue;
            }

            if (this.GetDefaultValues<T>().Contains(specificValue))
            {
                return upperLimitValue;
            }

            if (specificValue.CompareTo(upperLimitValue) <= 0)
            {
                return specificValue;
            }

            return upperLimitValue;
        }

        /// <inheritdoc/>
        public bool IsDefault<T>(T value)
        {
            var defaultValues = this.GetDefaultValues<T>();

            if (defaultValues.Contains(value))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void RegisterDefault<T>(T value, params T[] additionalValues)
        {
            if (!this.defaultValuesDic.ContainsKey(typeof(T)))
            {
                this.defaultValuesDic.Add(typeof(T), new List<object> { value });

                if (additionalValues.Any())
                {
                    this.defaultValuesDic[typeof(T)].AddRange(additionalValues.Cast<object>());
                }
            }
        }

        /// <summary>
        /// Returns a default value which was registered for <typeparamref name="T"/>.
        /// If no default value was registered the default value defined by the .Net
        /// framework is is returned.
        /// </summary>
        /// <typeparam name="T">Type for which the default value is retrieved.</typeparam>
        private IEnumerable<T> GetDefaultValues<T>()
        {
            T dotNetDefault = default;

            if (!this.defaultValuesDic.ContainsKey(typeof(T)))
            {
                return new List<T> { dotNetDefault };
            }

            this.defaultValuesDic.TryGetValue(typeof(T), out var values);

            return values.Cast<T>();
        }
    }
}
