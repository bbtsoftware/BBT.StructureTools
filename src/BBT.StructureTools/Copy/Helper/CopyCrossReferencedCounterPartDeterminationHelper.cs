namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Determines the correct counter part of a cross-referenced model within a copied structure,
    /// and sets the property on the referencing model accordingly.
    /// The "cross-referenced models" are registered in a repository when copied. So that when the
    /// "referencing models" are copied the correct counter part of the "cross-referenced model"
    /// can be determined and set.
    /// </summary>
    /// <typeparam name="TCrossReferencedModel">The type of the "cross-referenced model".</typeparam>
    /// <typeparam name="TReferencingModel">The type of the "referencing model".</typeparam>
    internal class CopyCrossReferencedCounterPartDeterminationHelper<TCrossReferencedModel, TReferencingModel>
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        /// <summary>
        /// Represents the property on the "referencing model" that refers to the "referenced model".
        /// </summary>
        private readonly PropertyInfo referencingProperty;

        /// <summary>
        /// Stores the post processing for the "cross-referenced model".
        /// </summary>
        private readonly GenericCopyPostProcessing<TCrossReferencedModel> crossReferencedModelPostProcessings;

        /// <summary>
        /// Stores the post processing for the "referencing model".
        /// </summary>
        private readonly GenericCopyPostProcessing<TReferencingModel> referencingModelPostProcessings;

        /// <summary>
        /// A repository of source target pairs of the copied "cross-referenced models".
        /// </summary>
        private readonly Dictionary<TCrossReferencedModel, TCrossReferencedModel> referencedModelSourceTargetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCrossReferencedCounterPartDeterminationHelper{TCrossReferencedModel,TReferencingModel}"/> class.
        /// </summary>
        /// <remarks>It is essential that the "cross-referenced models" are copied before the "referencing models".</remarks>
        /// <param name="referencingProperty">An expression representing the property on the "referencing model" that refers to the "cross-referenced model".</param>
        internal CopyCrossReferencedCounterPartDeterminationHelper(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty)
        {
            referencingProperty.NotNull(nameof(referencingProperty));

            this.referencingProperty = ExpressionUtils.GetProperty(referencingProperty);

            this.crossReferencedModelPostProcessings = new GenericCopyPostProcessing<TCrossReferencedModel>(this.CrossReferencedModelAction);
            this.referencingModelPostProcessings = new GenericCopyPostProcessing<TReferencingModel>(this.ReferencingModelAction);

            this.referencedModelSourceTargetRepository = new Dictionary<TCrossReferencedModel, TCrossReferencedModel>();
        }

        /// <summary>
        /// Loads the specified set of additional processing items with new ones, which will handle
        /// the determination of the counter part.
        /// </summary>
        /// <param name="additionalProcessings">Set of existing additional processing items.</param>
        internal void FillAdditionalProcessings(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.NotNull(nameof(additionalProcessings));

            additionalProcessings.Add(this.crossReferencedModelPostProcessings);
            additionalProcessings.Add(this.referencingModelPostProcessings);
        }

        /// <summary>
        /// Is called when the "cross-referenced models" are processed.
        /// Registers a source target pair in the repository, so the can later be resolved when the
        /// "referencing models" are processed.
        /// </summary>
        private void CrossReferencedModelAction(TCrossReferencedModel source, TCrossReferencedModel target)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            // Register source target pair.
            this.referencedModelSourceTargetRepository.Add(source, target);
        }

        /// <summary>
        /// Is called when the "referencing models" are processed.
        /// Determines the correct counter part for the property specified in the constructor.
        /// </summary>
        private void ReferencingModelAction(TReferencingModel source, TReferencingModel target)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            // Determine correct counter part.
            var crossReferencedSourceModel = (TCrossReferencedModel)this.referencingProperty.GetValue(source);

            if (crossReferencedSourceModel == null)
            {
                // abort, referencing property is null
                return;
            }

            if (!this.referencedModelSourceTargetRepository.TryGetValue(
                crossReferencedSourceModel,
                out var crossReferencedCounterPart))
            {
                var referencingType = typeof(TReferencingModel);
                var crossReferencedType = typeof(TCrossReferencedModel);

                throw new CopyConvertCompareException($"The correct cross-referenced counter part could not be determined. The referenced models ('{crossReferencedType}') must be processed before the referencing models ('{referencingType}')");
            }

            // Set correct counter part.
            this.referencingProperty.SetValue(target, crossReferencedCounterPart);
        }
    }
}
