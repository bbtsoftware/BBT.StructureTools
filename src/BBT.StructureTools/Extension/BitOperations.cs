// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System;

    /// <summary>
    /// Helps with bit operations.
    /// </summary>
    /// <remarks>
    /// Replace with https://docs.microsoft.com/en-us/dotnet/api/system.numerics.bitoperations?view=netcore-3.0
    /// as soon as it is available on .Net Standard.
    /// </remarks>
    public static class BitOperations
    {
        /// <summary>
        /// Circular shift to the left by <paramref name="aCount"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static uint RotateL(uint aValue, int aCount)
        {
            return (aValue << aCount) | (aValue >> checked(32 - aCount));
        }

        /// <summary>
        /// Circular shift to the left by <paramref name="aCount"/>.
        /// </summary>
        public static int RotateL(int aValue, int aCount)
        {
            checked
            {
                if (aValue < 0)
                {
                    return (aValue << aCount) | ((aValue >> (32 - aCount)) & ~(-1 << aCount));
                }

                return (aValue << aCount) | (aValue >> (32 - aCount));
            }
        }

        /// <summary>
        /// Circular shift to the right by <paramref name="aCount"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static uint RotateR(uint aValue, int aCount)
        {
            return (aValue >> aCount) | (aValue << checked(32 - aCount));
        }

        /// <summary>
        /// Circular shift to the right by <paramref name="aCount"/>.
        /// </summary>
        public static int RotateR(int aValue, int aCount)
        {
            checked
            {
                if (aValue < 0)
                {
                    return ((aValue >> aCount) & ~(-1 << (32 - aCount))) | (aValue << (32 - aCount));
                }

                return (aValue >> aCount) | (aValue << (32 - aCount));
            }
        }
    }
}
