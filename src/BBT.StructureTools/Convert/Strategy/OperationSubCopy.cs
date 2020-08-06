namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class OperationSubCopy<TSource, TTarget, TValue> : IOperationSubCopy<TSource, TTarget, TValue>
        where TSource : class, TValue
        where TTarget : class, TValue
        where TValue : class
    {
        private readonly ICopy<TValue> copy;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSubCopy{TSource,TTarget, TSubCopy}" /> class.
        /// </summary>
        public OperationSubCopy(
            ICopy<TValue> copy)
        {
            StructureToolsArgumentChecks.NotNull(copy, nameof(copy));

            this.copy = copy;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(target, nameof(target));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            this.copy.Copy(source, target, additionalProcessings);
        }
    }
}
