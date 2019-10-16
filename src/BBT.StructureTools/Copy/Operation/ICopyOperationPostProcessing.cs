namespace BBT.StructureTools.Copy.Operation
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// A copy operation which adds post processing operations which are executed at the end of the
    /// copy process.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the object being copied.
    /// </typeparam>
    internal interface ICopyOperationPostProcessing<in T> : ICopyOperation<T>
        where T : class
    {
        /// <summary>
        /// Initializes the operation with a ist of <see cref="IBaseAdditionalProcessing"/>.
        /// ist can be empty, but must not be null.
        /// </summary>
        void Initialize(Collection<IBaseAdditionalProcessing> additionalProcessings);
    }
}