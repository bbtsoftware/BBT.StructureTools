// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Determines the correct counter part of a cross-referenced model within a copied structure,
    /// and sets the property on the referencing model accordingly.
    /// The "cross-referenced models" are registered in a repository when copied. So that when the
    /// "referencing models" are copied the correct counter part of the "cross-referenced model"
    /// can be determined and set.
    /// </summary>
    /// <typeparam name="TCrossReferencedModel">The type of the "cross-referenced model".</typeparam>
    /// <typeparam name="TReferencingModel">The type of the "referencing model".</typeparam>
    public class CopyCrossReferencedCounterPartDeterminationHelper<TCrossReferencedModel, TReferencingModel>
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        /// <summary>
        /// Represents the property on the "referencing model" that refers to the "referenced model".
        /// </summary>
        private readonly PropertyInfo mReferencingProperty;

        /// <summary>
        /// Stores the post processing for the "cross-referenced model".
        /// </summary>
        private readonly GenericCopyPostProcessing<TCrossReferencedModel> mCrossReferencedModelPostProcessings;

        /// <summary>
        /// Stores the post processing for the "referencing model".
        /// </summary>
        private readonly GenericCopyPostProcessing<TReferencingModel> mReferencingModelPostProcessings;

        /// <summary>
        /// A repository of source target pairs of the copied "cross-referenced models".
        /// </summary>
        private readonly Dictionary<TCrossReferencedModel, TCrossReferencedModel> mReferencedModelSourceTargetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCrossReferencedCounterPartDeterminationHelper{TCrossReferencedModel,TReferencingModel}"/> class.
        /// </summary>
        /// <remarks>It is essential that the "cross-referenced models" are copied before the "referencing models".</remarks>
        /// <param name="aReferencingProperty">An expression representing the property on the "referencing model" that refers to the "cross-referenced model".</param>
        public CopyCrossReferencedCounterPartDeterminationHelper(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> aReferencingProperty)
        {
            aReferencingProperty.Should().NotBeNull();

            this.mReferencingProperty = ExpressionUtils.GetProperty(aReferencingProperty);

            this.mCrossReferencedModelPostProcessings = new GenericCopyPostProcessing<TCrossReferencedModel>(this.CrossReferencedModelAction);
            this.mReferencingModelPostProcessings = new GenericCopyPostProcessing<TReferencingModel>(this.ReferencingModelAction);

            this.mReferencedModelSourceTargetRepository = new Dictionary<TCrossReferencedModel, TCrossReferencedModel>();
        }

        /// <summary>
        /// Loads the specified set of additional processing items with new ones, which will handle
        /// the determination of the counter part.
        /// </summary>
        /// <param name="additionalProcessings">Set of existing additional processing items.</param>
        public void FillAdditionalProcessings(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            additionalProcessings.Add(this.mCrossReferencedModelPostProcessings);
            additionalProcessings.Add(this.mReferencingModelPostProcessings);
        }

        /// <summary>
        /// Is called when the "cross-referenced models" are processed.
        /// Registers a source target pair in the repository, so the can later be resolved when the
        /// "referencing models" are processed.
        /// </summary>
        private void CrossReferencedModelAction(TCrossReferencedModel source, TCrossReferencedModel target)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            // Register source target pair.
            this.mReferencedModelSourceTargetRepository.Add(source, target);
        }

        /// <summary>
        /// Is called when the "referencing models" are processed.
        /// Determines the correct counter part for the property specified in the constructer.
        /// </summary>
        private void ReferencingModelAction(TReferencingModel source, TReferencingModel target)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();

            // Determine correct counter part.
            var lCrossReferencedSourceModel = (TCrossReferencedModel)this.mReferencingProperty.GetValue(source);

            if (lCrossReferencedSourceModel == null)
            {
                // abort, referencing property is null
                return;
            }

            if (!this.mReferencedModelSourceTargetRepository.TryGetValue(
                lCrossReferencedSourceModel,
                out var lCrossReferencedCounterPart))
            {
                var lReferencingType = typeof(TReferencingModel);
                var lCrossReferencedType = typeof(TCrossReferencedModel);

                throw new CopyConvertCompareException($"The correct cross-referenced counter part could not be determined. The referenced models ('{lCrossReferencedType}') must be processed before the referencing models ('{lReferencingType}')");
            }

            // Set correct counter part.
            this.mReferencingProperty.SetValue(target, lCrossReferencedCounterPart);
        }
    }
}
