namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to add some additional functions on the beginning of the convert process.
    /// </summary>
    /// <typeparam name="TSourceClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertPreProcessing<in TSourceClass, in TTargetClass> : IBaseAdditionalProcessing
        where TSourceClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will called at the end of a convert process.
        /// </summary>
        void DoPreProcessing(TSourceClass source, TTargetClass target);
    }
}
