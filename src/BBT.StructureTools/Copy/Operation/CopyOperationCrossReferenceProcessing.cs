// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyOperationCrossReferenceProcessing{T,TCrossReferencedModel, TReferencingModel}"/> for documentation.
    /// </summary>
    /// <typeparam name="T">see link above.</typeparam>
    /// <typeparam name="TCrossReferencedModel">see link above.</typeparam>
    /// <typeparam name="TReferencingModel">see link above.</typeparam>
    public class CopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel> : ICopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel>
        where T : class
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        private Expression<Func<TReferencingModel, TCrossReferencedModel>> mReferencingProperyExpression;

        /// <summary>
        /// See <see cref="CopyOperationCrossReferenceProcessing{T, TCrossReferencedModel, TReferencingModel}.Initialize"/>.
        /// </summary>
        public void Initialize(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> aReferencingProperty)
        {
            aReferencingProperty.Should().NotBeNull();

            this.mReferencingProperyExpression = aReferencingProperty;
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            copyCallContext.Should().NotBeNull();

            var lCrossReferenceHandler = new CopyCrossReferencedCounterPartDeterminationHelper<TCrossReferencedModel, TReferencingModel>(this.mReferencingProperyExpression);
            lCrossReferenceHandler.FillAdditionalProcessings(copyCallContext.AdditionalProcessings);
        }
    }
}
