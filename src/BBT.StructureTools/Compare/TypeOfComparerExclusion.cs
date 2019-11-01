namespace BBT.StructureTools.Compare
{
    /// <summary>
    /// Define the different types of exclusion.
    /// </summary>
    public enum TypeOfComparerExclusion
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        /// A property is excluded
        /// </summary>
        Property = 1,

        /// <summary>
        /// A sub interface is excluded
        /// </summary>
        SubInterface = 2,
    }
}