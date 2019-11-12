namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to add some additional functions on the end of the convert process.
    /// </summary>
    /// <typeparam name="TSourceClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertPostProcessing<TSourceClass, TTargetClass> : IBaseAdditionalProcessing
        where TSourceClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will be called at the end of a convert process.
        /// </summary>
        void DoPostProcessing(TSourceClass source, TTargetClass target);
    }
}
