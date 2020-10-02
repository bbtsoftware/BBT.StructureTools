namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Implementation of <see cref="IOperationConvertPostProcessing{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Source object type.</typeparam>
    /// <typeparam name="TTarget">Target object name.</typeparam>
    internal class OperationConvertPostProcessing<TSource, TTarget> : IOperationConvertPostProcessing<TSource, TTarget>
            where TSource : class
            where TTarget : class
    {
        private ICollection<IBaseAdditionalProcessing> additionalProcessings;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertPostProcessing{TSource, TTarget}" /> class.
        /// </summary>
        public OperationConvertPostProcessing(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.NotNull(nameof(additionalProcessings));

            this.additionalProcessings = additionalProcessings;
        }

        /// <summary>
        /// <see cref="IConvertOperation{TSource, TTarget}.Execute(TSource, TTarget, ICollection{IBaseAdditionalProcessing})"/>.
        /// </summary>
        public void Execute(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.NotNull(nameof(additionalProcessings));

            additionalProcessings.AddRangeToMe(this.additionalProcessings);
        }
    }
}
