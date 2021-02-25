namespace BBT.StructureTools.Convert.Strategy
{
    using System.Collections.Generic;

    /// <summary>
    /// The single unit of a convert helper operation.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public interface IConvertOperation<in TSource, in TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Performs the operation unit.
        /// </summary>
        void Execute(
            TSource source,
            TTarget target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
