namespace BBT.StructureTools.Compare.Exclusions
{
    using System;

    /// <inheritdoc/>
    public sealed class SubInterfaceComparerExclusion<TSubInterface> : IComparerExclusion
    {
        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        private readonly TypeOfComparerExclusion typeOfComparerExclusion;

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        private readonly Type excludedModelType;

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        private readonly string excludedPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubInterfaceComparerExclusion{TSubInterface}"/> class.
        /// </summary>
        public SubInterfaceComparerExclusion()
        {
            this.excludedModelType = typeof(TSubInterface);
            this.excludedPropertyName = string.Empty;
            this.typeOfComparerExclusion = TypeOfComparerExclusion.SubInterface;
        }

        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        public TypeOfComparerExclusion TypeOfComparerExclusion
        {
            get
            {
                return this.typeOfComparerExclusion;
            }
        }

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        public Type ExcludedModelType
        {
            get
            {
                return this.excludedModelType;
            }
        }

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        public string ExcludedPropertyName
        {
            get
            {
                return this.excludedPropertyName;
            }
        }
    }
}
