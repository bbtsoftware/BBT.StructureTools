namespace BBT.StructureTools.Compare
{
    using System;

    /// <summary>
    /// Interface for <see cref="IComparer{TModel, TComparerIntention}"/> exclusions.
    /// </summary>
    public interface IComparerExclusion
    {
        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        TypeOfComparerExclusion TypeOfComparerExclusion { get; }

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        Type ExcludedModelType { get; }

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        string ExcludedPropertyName { get; }
    }
}