// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// See <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="T">Owner of the attribute to copy.</typeparam>
    /// <typeparam name="TValue">The value type to copy.</typeparam>
    internal class CopyOperation<T, TValue> : ICopyOperation<T>
    {
        private readonly Func<T, TValue> func;
        private readonly string propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperation{T,TValue}"/> class.
        /// </summary>
        public CopyOperation(Expression<Func<T, TValue>> expression)
        {
            this.func = expression.Compile();
            this.propertyName = ReflectionUtils.GetPropertyName(expression);
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var value = this.func.Invoke(source);

            var targetProperty = target.GetType().GetProperty(this.propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (targetProperty != null)
            {
                targetProperty.SetValue(target, value, null);
            }
        }
    }
}