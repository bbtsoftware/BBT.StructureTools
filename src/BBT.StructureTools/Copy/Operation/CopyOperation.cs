namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
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

        /// <inheritdoc/>
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