namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationCopySource<TSource, TTarget>
        : IOperationCopySource<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private Expression<Func<TTarget, TSource>> targetexpression;

        /// <inheritdoc/>
        public void Initialize(Expression<Func<TTarget, TSource>> targetExpression)
        {
            targetExpression.NotNull(nameof(targetExpression));

            this.targetexpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            target.SetPropertyValue(this.targetexpression, source);
        }
    }
}
