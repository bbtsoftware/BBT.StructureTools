namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <inheritdoc/>
    internal class OperationCopyValueWithLookUp<TSource, TTarget, TValue>
        : IOperationCopyValueWithLookUp<TSource, TTarget, TValue>
        where TSource : class
        where TTarget : class
    {
        private readonly IDefaultValueProvider defaultValueProvider;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TSource, TValue> sourceFunc;

        /// <summary>
        /// Function to get the look-up value.
        /// </summary>
        private Func<TSource, TValue> sourceLookUpFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Expression<Func<TTarget, TValue>> targetExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCopyValueWithLookUp{TSource, TTarget, TValue}"/> class.
        /// </summary>
        public OperationCopyValueWithLookUp(
            IDefaultValueProvider defaultValueProvider)
        {
            defaultValueProvider.NotNull(nameof(defaultValueProvider));

            this.defaultValueProvider = defaultValueProvider;
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceLookUpFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            sourceLookUpFunc.NotNull(nameof(sourceLookUpFunc));
            targetExpression.NotNull(nameof(targetExpression));

            this.sourceFunc = sourceFunc;
            this.sourceLookUpFunc = sourceLookUpFunc;
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
            if (this.defaultValueProvider.IsDefault(sourceValue))
            {
                sourceValue = this.sourceLookUpFunc.Invoke(source);
            }

            target.SetPropertyValue(
                this.targetExpression,
                sourceValue);
        }
    }
}
