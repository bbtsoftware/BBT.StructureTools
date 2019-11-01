namespace BBT.StructureTools.Compare
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IComparer{T}"/>.
    /// Compares two types based on how they inherit each other / are assignable to each other.
    /// A more specific type is concidered "higher up" in the inheritence hierarchy, and therefore
    /// "greater" in a comparison.
    /// </summary>
    public sealed class TypeComparer : IComparer<Type>
    {
        /// <inheritdoc/>
        public int Compare(Type x, Type y)
        {
            x.Should().NotBeNull();
            y.Should().NotBeNull();

            // ' X and Y are the same type.
            if (x == y)
            {
                return 0;
            }

            if (x.IsAssignableFrom(y))
            {
                // ' Means x is less.
                return -1;
            }

            if (y.IsAssignableFrom(x))
            {
                // ' Means x is greater.
                return 1;
            }

            // ' If they are completely unrelated, none is greater.
            return 0;
        }
    }
}
