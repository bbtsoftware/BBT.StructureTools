namespace BBT.StructureTools.Copy
{
    using BBT.StructureTools.Copy.Marker;

    /// <summary>
    /// Represents a "copy" process for an object which refers to its parent.
    /// </summary>
    /// <remarks>
    /// This is not supported by the "copy infrastructure" atm, and can only be implemented directly as needed. See https://jira.bbtsoftware.ch/bbt/browse/BBTL-4614.
    /// </remarks>
    /// <typeparam name="TClassToCopy">
    /// Type which is copied.
    /// </typeparam>
    /// <typeparam name="TParent">
    /// Type of the parent object.
    /// </typeparam>
    public interface ICopyWithParent<in TClassToCopy, TParent> : ICopyMarker
        where TClassToCopy : class
        where TParent : class
    {
        /// <summary>
        /// Does the copy.
        /// </summary>
        void Copy(
            TClassToCopy source,
            TClassToCopy target,
            TParent parent,
            ICopyCallContext copyCallContext);
    }
}
