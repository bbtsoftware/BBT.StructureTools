namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;

    using BBT.StructureTools.Copy.Marker;
    using BBT.StructureTools.Copy.Operation;

    /// <summary>
    /// Interface to create a copy from a source model to a target.
    /// Do not implement this class for a specific type, use the generic implementation which
    /// works with <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="TClassToCopy">Type of model.</typeparam>
    public interface ICopy<in TClassToCopy> : ICopyMarker
        where TClassToCopy : class
    {
        /// <summary>
        /// Do the copy. TODO: remove this overload.
        /// </summary>
        void Copy(
            TClassToCopy source,
            TClassToCopy target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);

        /// <summary>
        /// Do the copy.
        /// </summary>
        void Copy(
            TClassToCopy source,
            TClassToCopy target,
            ICopyCallContext copyCallContext);
    }
}
