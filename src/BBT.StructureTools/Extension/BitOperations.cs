namespace BBT.StructureTools.Extension
{
    /// <summary>
    /// Helps with bit operations.
    /// </summary>
    /// <remarks>
    /// Replace with https://docs.microsoft.com/en-us/dotnet/api/system.numerics.bitoperations?view=netcore-3.0
    /// as soon as it is available on .Net Standard.
    /// </remarks>
    internal static class BitOperations
    {
        /// <summary>
        /// Circular shift to the left by <paramref name="count"/>.
        /// </summary>
        internal static int RotateL(int value, int count)
        {
            checked
            {
                if (value < 0)
                {
                    return (value << count) | ((value >> (32 - count)) & ~(-1 << count));
                }

                return (value << count) | (value >> (32 - count));
            }
        }
    }
}
