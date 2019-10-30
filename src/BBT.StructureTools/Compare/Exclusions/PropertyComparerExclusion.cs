// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Compare.Exclusions
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Compare;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IComparerExclusion"/>.
    /// </summary>
    /// <typeparam name="TModelInterface">Type of model interface.</typeparam>
    public sealed class PropertyComparerExclusion<TModelInterface> : IComparerExclusion
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
        /// Initializes a new instance of the <see cref="PropertyComparerExclusion{TModelInterface}"/> class.
        /// </summary>
        public PropertyComparerExclusion(Expression<Func<TModelInterface, object>> aExclusion)
        {
            aExclusion.Should().NotBeNull();

            this.mExcludedModelType = typeof(TModelInterface);

            if (aExclusion.Body is MemberExpression lMemberExpression)
            {
                this.mExcludedPropertyName = lMemberExpression.Member.Name;
            }
            else
            {
                var lUnaryExpression = aExclusion.Body as UnaryExpression;
                lMemberExpression = (MemberExpression)lUnaryExpression.Operand;
                this.mExcludedPropertyName = lMemberExpression.Member.Name;
            }

            this.mTypeOfComparerExclusion = TypeOfComparerExclusion.Property;
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
