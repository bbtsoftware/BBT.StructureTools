namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
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
            copy.NotNull(nameof(copy));

            this.copy = copy;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            aTarget.NotNull(nameof(aTarget));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            this.copy.Copy(source, aTarget, additionalProcessings);
        }
    }
}
