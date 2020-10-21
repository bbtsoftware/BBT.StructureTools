namespace BBT.StructureTools.Impl.Compare.Helper
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// See <see cref="IComparer{T}"/>.
    /// Compares two types based on how they inherit each other / are assignable to each other.
    /// A more specific type is concidered "higher up" in the inheritence hierarchy, and therefore
    /// "greater" in a comparison.
    /// </summary>
    internal class TypeComparer : IComparer<Type>
    {
        /// <inheritdoc/>
        public int Compare(Type aX, Type aY)
        {
            aX.NotNull(nameof(aX));
            aY.NotNull(nameof(aY));

            // Special case: x and y are the same type.
            if (aX == aY)
            {
                return 0;
            }

            if (aX.IsAssignableFrom(aY))
            {
                // Means x is less.
                return -1;
            }

            if (aY.IsAssignableFrom(aX))
            {
                // Means x is greater.
                return 1;
            }

            // If they are completely unrelated, none is greater.
            return 0;
        }
    }
}
