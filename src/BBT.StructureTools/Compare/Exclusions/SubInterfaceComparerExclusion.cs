namespace BBT.StructureTools.Compare.Exclusions
{
    using System;

    /// <inheritdoc/>
    public sealed class SubInterfaceComparerExclusion<TSubInterface> : IComparerExclusion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubInterfaceComparerExclusion{TSubInterface}"/> class.
        /// </summary>
        public SubInterfaceComparerExclusion()
        {
            this.ExcludedModelType = typeof(TSubInterface);
            this.ExcludedPropertyName = string.Empty;
            this.TypeOfComparerExclusion = TypeOfComparerExclusion.SubInterface;
        }

        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        public TypeOfComparerExclusion TypeOfComparerExclusion { get; }

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        public Type ExcludedModelType { get; }

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        public string ExcludedPropertyName { get; }
    }
}
