namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class CopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel> : ICopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel>
        where T : class
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        private Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperyExpression;

        /// <inheritdoc/>
        public void Initialize(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty)
        {
            referencingProperty.Should().NotBeNull();

            this.referencingProperyExpression = referencingProperty;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            copyCallContext.Should().NotBeNull();

            var crossReferenceHandler = new CopyCrossReferencedCounterPartDeterminationHelper<TCrossReferencedModel, TReferencingModel>(this.referencingProperyExpression);
            crossReferenceHandler.FillAdditionalProcessings(copyCallContext.AdditionalProcessings);
        }
    }
}
