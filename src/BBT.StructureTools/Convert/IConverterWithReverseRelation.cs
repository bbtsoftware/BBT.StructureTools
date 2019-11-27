namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to convert from a source model to a target model.
    /// </summary>
    /// <typeparam name="TSourceClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    /// <typeparam name="TReverseRelation">Type of reverse relation.</typeparam>
    /// <typeparam name="TConvertIntention">Type of convert intention.</typeparam>
    public interface IConverterWithReverseRelation<in TSourceClass, in TTargetClass, in TReverseRelation, TConvertIntention>
        where TSourceClass : class
        where TTargetClass : class
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Do the convert.
        /// </summary>
        void Convert(
            TSourceClass source,
            TTargetClass target,
            TReverseRelation reverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
