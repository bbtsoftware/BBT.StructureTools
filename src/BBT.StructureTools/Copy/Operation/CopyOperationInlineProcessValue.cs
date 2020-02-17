namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationInlineProcessValue<T, TValue> : ICopyOperation<T>
        where T : class
    {
        private readonly Expression<Func<T, TValue>> targetExpression;
        private readonly Func<TValue> valueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationInlineProcessValue{T,TValue}"/> class.
        /// </summary>
        public CopyOperationInlineProcessValue(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TValue>> attrValueExpression)
        {
            targetExpression.NotNull(nameof(targetExpression));
            attrValueExpression.NotNull(nameof(attrValueExpression));

            this.targetExpression = targetExpression;
            this.valueFunc = attrValueExpression.Compile();
        }

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            target.SetPropertyValue(this.targetExpression, this.valueFunc.Invoke());
        }
    }
}