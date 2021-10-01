namespace BBT.StructureTools.Extensions.Convert
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;

    /// <summary>
    /// Strategy to provide methods for creation of a concrete <typeparamref name="TTarget"/>
    /// and conversion of a concrete <typeparamref name="TSource"/> into a concrete <typeparamref name="TTarget"/>.
    /// See also <see cref="CreateConvertStrategy{TSource, TConcreteSource, TTarget, TConcreteTarget, TConcreateTargetImpl, TIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of source.</typeparam>
    /// <typeparam name="TTarget">The type of target.</typeparam>
    /// <typeparam name="TIntention">The convert intention.</typeparam>
    public interface ICreateConvertStrategy<TSource, TTarget, TIntention>
        : IGenericStrategy<TSource>
        where TSource : class
        where TTarget : class
        where TIntention : IBaseConvertIntention
    {
        /// <summary>
        /// Creates a concrete <typeparamref name="TTarget"/>.
        /// </summary>
        TTarget CreateTarget(TSource aSource);

        /// <summary>
        /// Converts a concrete <typeparamref name="TSource"/> into a concrete <typeparamref name="TTarget"/>.
        /// </summary>
        void Convert(TSource aSource, TTarget aTraget, ICollection<IBaseAdditionalProcessing> aAdditionalProcessings);
    }
}