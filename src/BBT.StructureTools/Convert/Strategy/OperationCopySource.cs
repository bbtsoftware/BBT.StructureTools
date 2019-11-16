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
        private Expression<Func<TTarget, TSource>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopySource{TSource, TTarget}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public OperationCopySource()
        {
        }

        /// <inheritdoc/>
        public void Initialize(Expression<Func<TTarget, TSource>> targetExpression)
        {
            targetExpression.NotNull(nameof(targetExpression));

            this.targetExpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            target.SetPropertyValue(this.targetExpression, source);
        }
    }
}
