namespace BBT.StructureTools.Constants
{
    /// <summary>
    /// Contains constant values regarding temporal data.
    /// </summary>
    internal static class TemporalConstants
    {
        /// <summary>
        /// Gets the <see cref="System.DateTime"/> value (a constant) which represents
        /// an "infinite" date in a time range.
        /// </summary>
        internal static System.DateTime InfiniteDate { get; } = new System.DateTime(9999, 12, 31);
    }
}
