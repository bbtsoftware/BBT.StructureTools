namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel> : ICopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel>
        where T : class
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        private Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingPropertyExpression;

        /// <inheritdoc/>
        public void Initialize(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty)
        {
            referencingProperty.NotNull(nameof(referencingProperty));

            this.referencingPropertyExpression = referencingProperty;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            copyCallContext.NotNull(nameof(copyCallContext));

            var crossReferenceHandler = new CopyCrossReferencedCounterPartDeterminationHelper<TCrossReferencedModel, TReferencingModel>(this.referencingPropertyExpression);
            crossReferenceHandler.FillAdditionalProcessings(copyCallContext.AdditionalProcessings);
        }
    }
}
