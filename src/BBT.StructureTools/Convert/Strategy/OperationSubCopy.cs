// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IOperationSubCopy{TSource, TTarget, TSubCopy}"/>.
    /// </summary>
    /// <typeparam name="TSource">see link above.</typeparam>
    /// <typeparam name="TTarget">see link above.</typeparam>
    /// <typeparam name="TValue">see link above.</typeparam>
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

        /// <summary>
        /// See <see cref="IConvertOperation{TSource,TTarget}.Execute"/>.
        /// </summary>
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
