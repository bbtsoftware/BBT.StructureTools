namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to intercept the convert process.
    /// </summary>
    /// <typeparam name="TSourceClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertInterception<in TSourceClass, in TTargetClass> : IBaseAdditionalProcessing
        where TSourceClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will called before the convert process of the model in the type parameter starts.
        /// </summary>
        /// <returns><code>True</code> if the model must not convert, otherwise. <code>False</code></returns>
        bool CallConverter(TSourceClass source);
    }
}
