namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to convert from a source model to a target model.
    /// </summary>
    /// <typeparam name="TSourceClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    /// <typeparam name="TConvertIntention">Type of convert intention.</typeparam>
    public interface IConvert<in TSourceClass, in TTargetClass, TConvertIntention>
        where TSourceClass : class
        where TTargetClass : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Do the convert.
        /// </summary>
        void Convert(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
