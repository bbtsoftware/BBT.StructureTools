namespace BBT.StructureTools.Copy.Processing
{
    /// <summary>
    /// Interface to intercept the copy process.
    /// </summary>
    /// <typeparam name="TType">Typisation here.</typeparam>
    internal interface IGenericContinueCopyInterception<in TType> : IBaseAdditionalProcessing
        where TType : class
    {
        /// <summary>
        /// Returns true if the object shall be copied.
        /// </summary>
        bool ShallCopy(TType @object);
    }
}