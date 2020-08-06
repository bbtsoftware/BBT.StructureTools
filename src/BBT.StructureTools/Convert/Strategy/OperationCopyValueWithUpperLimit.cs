namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        : IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
        where TValue : IComparable<TValue>
    {
        private readonly IDefaultValueProvider defaultValueProvider;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> sourceFunc;

        /// <summary>
        /// Function to get the look-up value.
        /// </summary>
        private Func<TSource, TValue> sourceUpperLimitFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithUpperLimit{TSource, TTarget, TValue}"/> class.
        /// </summary>
        public OperationCopyValueWithUpperLimit(
            IDefaultValueProvider defaultValueProvider)
        {
            StructureToolsArgumentChecks.NotNull(defaultValueProvider, nameof(defaultValueProvider));

            this.defaultValueProvider = defaultValueProvider;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(sourceUpperLimitFunc, nameof(sourceUpperLimitFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            this.sourceFunc = sourceFunc;
            this.sourceUpperLimitFunc = sourceUpperLimitFunc;
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
            var upperLimitValue = this.sourceUpperLimitFunc(source);

            var value = this.defaultValueProvider.ApplyUpperLimit(sourceValue, upperLimitValue);
            target.SetPropertyValue(
                this.targetExpression,
                value);
        }
    }
}
