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
        private readonly Func<T, TValue> mFunc;
        private readonly string mPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperation{T,TValue}"/> class.
        /// </summary>
        public CopyOperation(Expression<Func<T, TValue>> aExpression)
        {
            this.mFunc = aExpression.Compile();
            this.mPropertyName = ReflectionUtils.GetPropertyName(aExpression);
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var lValue = this.mFunc.Invoke(source);

            var lTargetProperty = target.GetType().GetProperty(this.mPropertyName, BindingFlags.Public | BindingFlags.Instance);

            if (lTargetProperty != null)
            {
                lTargetProperty.SetValue(target, lValue, null);
            }
        }
    }
}