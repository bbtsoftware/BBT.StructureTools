// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TSourceValue">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class OperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>
        : IOperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TSourceValue : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IConvert<TSourceValue, TTarget, TConvertIntention> mConvert;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSourceSubConvert{TSource,TTarget,TSourceValue,TConvertIntention}" /> class.
        /// </summary>
        public OperationSourceSubConvert(
            IConvert<TSourceValue, TTarget, TConvertIntention> aConvert)
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
            if (source is TSourceValue lSourceValue)
            {
                this.mConvert.Convert(lSourceValue, target, additionalProcessings);
            }
        }
    }
}
