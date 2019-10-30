namespace BBT.StructureTools.Compare.Exclusions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// See <see cref="IComparerExclusion"/>.
    /// </summary>
    /// <typeparam name="TSubInterface">Type of sub interface.</typeparam>
    public sealed class SubInterfaceComparerExclusion<TSubInterface> : IComparerExclusion
    {
        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        private readonly TypeOfComparerExclusion mTypeOfComparerExclusion;

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        private readonly Type mExcludedModelType;

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        private readonly string mExcludedPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubInterfaceComparerExclusion{TSubInterface}"/> class.
        /// </summary>
        public SubInterfaceComparerExclusion()
        {
            this.mExcludedModelType = typeof(TSubInterface);
            this.mExcludedPropertyName = string.Empty;
            this.mTypeOfComparerExclusion = TypeOfComparerExclusion.SubInterface;
        }

        /// <summary>
        /// Gets the type of exclusion.
        /// </summary>
        public TypeOfComparerExclusion TypeOfComparerExclusion
        {
            get
            {
                return this.mTypeOfComparerExclusion;
            }
        }

        /// <summary>
        /// Gets the excluded model type.
        /// </summary>
        public Type ExcludedModelType
        {
            get
            {
                return this.mExcludedModelType;
            }
        }

        /// <summary>
        /// Gets the excluded property.
        /// </summary>
        public string ExcludedPropertyName
        {
            get
            {
                return this.mExcludedPropertyName;
            }
        }
    }
}
