namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A copy operation which adds cross referenceprocessing operations while perfoming the copy
    /// procedure.
    /// </summary>
    /// <typeparam name="T">Type of the object being copied.</typeparam>
    /// <typeparam name="TCrossReferencedModel">Type of cross referenced model.</typeparam>
    /// <typeparam name="TReferencingModel">Type of the object being copied.</typeparam>
    public interface ICopyOperationCrossReferenceProcessing<in T, TCrossReferencedModel, TReferencingModel> : ICopyOperation<T>
        where T : class
        where TCrossReferencedModel : class
        where TReferencingModel : class
    {
        /// <summary>
        /// Initializes the operation with a ist of <see cref="IBaseAdditionalProcessing"/>.
        /// ist can be empty, but must not be null.
        /// </summary>
        void Initialize(Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty);
    }
}