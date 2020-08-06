namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCopyValueIfSourceNotDefault<TSource, TTarget, TValue>
        : IOperationCopyValueIfSourceNotDefault<TSource, TTarget, TValue>
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
        /// Initializes a new instance of the <see cref="OperationCopyValueIfSourceNotDefault{TSource, TTarget, TValue}"/> class.
        /// </summary>
        public OperationCopyValueIfSourceNotDefault(
            IDefaultValueProvider defaultValueProvider)
        {
            StructureToolsArgumentChecks.NotNull(defaultValueProvider, nameof(defaultValueProvider));

            this.defaultValueProvider = defaultValueProvider;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            this.sourceFunc = sourceFunc;
            this.targetExpression = targetExpression;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));

            var sourceValue = this.sourceFunc.Invoke(source);
            if (this.defaultValueProvider.IsDefault(sourceValue))
            {
                return;
            }

            target.SetPropertyValue(
                this.targetExpression,
                sourceValue);
        }
    }
}
