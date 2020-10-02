namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        : IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private readonly IDefaultValueProvider defaultValueProvider;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueIfTargetIsDefault{TSource, TTarget, TValue}"/> class.
        /// </summary>
        public OperationCopyValueIfTargetIsDefault(
            IDefaultValueProvider defaultValueProvider)
        {
            defaultValueProvider.NotNull(nameof(defaultValueProvider));

            this.defaultValueProvider = defaultValueProvider;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));

            this.sourceFunc = sourceFunc;
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

            var sourceValue = this.sourceFunc.Invoke(source);
            var targetValue = target.GetPropertyValue(this.targetExpression);
            if (!this.defaultValueProvider.IsDefault<TValue>(targetValue))
            {
                return;
            }

            target.SetPropertyValue(
                this.targetExpression,
                sourceValue);
        }
    }
}
