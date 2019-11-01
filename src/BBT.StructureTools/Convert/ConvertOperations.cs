namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert.Strategy;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IConvertOperations{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    public class ConvertOperations<TSource, TTarget> : IConvertOperations<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// The ist of work units to be processed.
        /// </summary>
        private readonly IEnumerable<IConvertOperation<TSource, TTarget>> convertHelperOperationWorkUnits;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertOperations{TSource,TTarget}" /> class.
        /// </summary>
        public ConvertOperations(
            IEnumerable<IConvertOperation<TSource, TTarget>> convertHelperOperationWorkUnits)
        {
            convertHelperOperationWorkUnits.Should().NotBeNull();

            this.convertHelperOperationWorkUnits = convertHelperOperationWorkUnits;
        }

        /// <summary>
        /// See <see cref="IConvertOperations{TSource,TTarget}.Convert"/>.
        /// </summary>
        public void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            foreach (var convertHelperOperationWorkUnit in this.convertHelperOperationWorkUnits)
            {
                convertHelperOperationWorkUnit.Execute(source, target, additionalProcessings);
            }
        }
    }
}
