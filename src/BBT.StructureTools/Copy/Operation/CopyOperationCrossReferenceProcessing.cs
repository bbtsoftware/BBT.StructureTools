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

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCrossReferenceProcessing{T, TCrossReferencedModel, TReferencingModel}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public CopyOperationCrossReferenceProcessing()
        {
        }

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
