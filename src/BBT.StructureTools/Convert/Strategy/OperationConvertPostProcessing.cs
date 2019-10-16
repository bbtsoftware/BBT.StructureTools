// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Implementation of <see cref="IOperationConvertPostProcessing{TSource, TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">Source object type.</typeparam>
    /// <typeparam name="TTarget">Target object name.</typeparam>
    public class OperationConvertPostProcessing<TSource, TTarget> : IOperationConvertPostProcessing<TSource, TTarget>
            where TSource : class
            where TTarget : class
    {
        private ICollection<IBaseAdditionalProcessing> mAdditionalProcessings;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConvertPostProcessing{TSource, TTarget}" /> class.
        /// </summary>
        public OperationConvertPostProcessing(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            this.mAdditionalProcessings = additionalProcessings;
        }

        /// <summary>
        /// <see cref="IConvertOperation{TSource, TTarget}.Execute(TSource, TTarget, ICollection{IBaseAdditionalProcessing})"/>.
        /// </summary>
        public void Execute(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            additionalProcessings.AddRangeToMe(this.mAdditionalProcessings);
        }
    }
}
