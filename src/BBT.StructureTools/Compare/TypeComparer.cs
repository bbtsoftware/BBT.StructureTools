// Copyright © BBT Software AG. All rights reserved.

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
        /// <summary>
        /// See <see cref="IComparer{T}.Compare(T, T)"/>.
        /// </summary>
        public int Compare(Type aX, Type aY)
        {
            aX.Should().NotBeNull();
            aY.Should().NotBeNull();

            // ' X and Y are the same type.
            if (aX == aY)
            {
                return 0;
            }

            if (aX.IsAssignableFrom(aY))
            {
                // ' Means x is less.
                return -1;
            }

            if (aY.IsAssignableFrom(aX))
            {
                // ' Means x is greater.
                return 1;
            }

            // ' If they are completely unrelated, none is greater.
            return 0;
        }
    }
}
