namespace BBT.StructureTools.Convert.Strategy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using FluentAssertions;

    /// <inheritdoc/>
    public class OperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> convert;
        private Func<TTarget, TSourceValue> sourceFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertFromTargetOnDifferentLevels{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationConvertFromTargetOnDifferentLevels(
            IConvert<TSourceValue, TTarget, TConvertIntention> convert)
        {
            convert.Should().NotBeNull();

            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Initialize(Func<TTarget, TSourceValue> sourceFunc)
        {
            sourceFunc.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
        }

        /// <inheritdoc/>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            additionalProcessings.Add(
                new GenericConvertPostProcessing<TSource, TTarget>(
                    (x, y) => this.convert.Convert(this.sourceFunc(y), y, additionalProcessings)));
        }
    }
}
