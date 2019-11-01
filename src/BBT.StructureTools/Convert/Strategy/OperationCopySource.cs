namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationCopySource<TSource, TTarget>
        : IOperationCopySource<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private Expression<Func<TTarget, TSource>> targetexpression;

        /// <inheritdoc/>
        public void Initialize(Expression<Func<TTarget, TSource>> targetExpression)
        {
            targetExpression.Should().NotBeNull();

            this.targetexpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            target.SetPropertyValue(this.targetexpression, source);
        }
    }
}
