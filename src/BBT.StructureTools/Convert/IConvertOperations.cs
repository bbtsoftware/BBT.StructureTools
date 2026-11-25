namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;

    /// <summary>
    /// Operations which must be processed during conversion.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public interface IConvertOperations<in TSource, in TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Converts <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        void Convert(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
