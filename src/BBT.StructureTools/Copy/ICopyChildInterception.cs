namespace BBT.StructureTools.Copy
{
    /// <summary>
    /// Interface to intercept the copy process.
    /// </summary>
    /// <typeparam name="TClassToCopy">Type of the copied class.</typeparam>
    public interface ICopyChildInterception<in TClassToCopy> : IBaseAdditionalProcessing
        where TClassToCopy : class
    {
        /// <summary>
        /// This method will be called before the copy process of the model in the type parameter starts.
        /// </summary>
        /// <returns><code>True</code> if the model must not copy, otherwise. <code>False</code></returns>
        bool ShallCopyChild(TClassToCopy source, TClassToCopy target);
    }
}
