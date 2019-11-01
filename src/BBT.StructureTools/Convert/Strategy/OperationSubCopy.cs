namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationSubCopy<TSource, TTarget, TValue> : IOperationSubCopy<TSource, TTarget, TValue>
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
            copy.Should().NotBeNull();

            this.copy = copy;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget aTarget,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            aTarget.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            this.copy.Copy(source, aTarget, additionalProcessings);
        }
    }
}
