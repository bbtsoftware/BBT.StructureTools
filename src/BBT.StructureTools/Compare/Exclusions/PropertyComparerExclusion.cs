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
        /// Initializes a new instance of the <see cref="PropertyComparerExclusion{TModelInterface}"/> class.
        /// </summary>
        public PropertyComparerExclusion(Expression<Func<TModelInterface, object>> exclusion)
        {
            exclusion.Should().NotBeNull();

            this.ExcludedModelType = typeof(TModelInterface);

            if (exclusion.Body is MemberExpression memberExpression)
            {
                this.ExcludedPropertyName = memberExpression.Member.Name;
            }
            else
            {
                var unaryExpression = exclusion.Body as UnaryExpression;
                memberExpression = (MemberExpression)unaryExpression.Operand;
                this.ExcludedPropertyName = memberExpression.Member.Name;
            }

            this.TypeOfComparerExclusion = TypeOfComparerExclusion.Property;
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
