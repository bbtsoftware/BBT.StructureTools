// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationSubConvert{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TTargetValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        : IOperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TTargetValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTargetValue, TConvertIntention> mConvert;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSubConvert{TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention}" /> class.
        /// </summary>
        public OperationSubConvert(
            IConvert<TSourceValue, TTargetValue, TConvertIntention> aConvert)
        {
            aConvert.Should().NotBeNull();

            this.mConvert = aConvert;
        }

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
        public void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            // ToDo BBTL-5178: IF sollte nicht mehr nötig sein, wenn die entsprechenden
            // Registrierungen und Implementation die korrekte Basis aufrufen!
            var lTargetValue = target as TTargetValue;
            if (source is TSourceValue lSourceValue && lTargetValue != null)
            {
                this.mConvert.Convert(lSourceValue, lTargetValue, additionalProcessings);
            }
        }
    }
}
