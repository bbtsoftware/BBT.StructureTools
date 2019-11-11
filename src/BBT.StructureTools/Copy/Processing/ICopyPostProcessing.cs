namespace BBT.StructureTools.Copy.Processing
{
    /// <summary>
    /// Interface to add some additional functions on the end of the convert process.
    /// </summary>
    /// <typeparam name="TClassToCopy">Type of to copied class.</typeparam>
    public interface ICopyPostProcessing<TClassToCopy> : IBaseAdditionalProcessing
        where TClassToCopy : class
    {
        /// <summary>
        /// This method will called at the end of a copy process.
        /// </summary>
        void DoPostProcessing(TClassToCopy source, TClassToCopy target);
    }
}
