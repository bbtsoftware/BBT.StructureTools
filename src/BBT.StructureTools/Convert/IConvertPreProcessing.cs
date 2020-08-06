namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to add some additional functions on the beginning of the convert process.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertPreProcessing<in TSoureClass, in TTargetClass> : IBaseAdditionalProcessing
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will called at the end of a convert process.
        /// </summary>
        void DoPreProcessing(TSoureClass source, TTargetClass target);
    }
}
