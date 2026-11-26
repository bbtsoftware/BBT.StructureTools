namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the context of a specific "copy" call -- i.e. an instance of this is passed through the
    /// "copy stack".
    /// </summary>
    public interface ICopyCallContext
    {
        /// <summary>
        /// Gets the collection of "additional processings" for the current "copy" call.
        /// </summary>
        ICollection<IBaseAdditionalProcessing> AdditionalProcessings { get; }
    }
}
