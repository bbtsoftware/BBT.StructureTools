namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// A helper strategy used to copy attributes where the value is retrieved from a factory
    /// and retrieved during the copy process.
    /// </summary>
    /// <typeparam name="T">type to copy of the attribute.</typeparam>
    /// <typeparam name="TValue">Type of the attribute being inline processed / "copied".</typeparam>
    /// <typeparam name="TAttributeValueFactory">interface type of the attribute value factory.</typeparam>
    internal interface ICopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>
        : ICopyOperation<T>
        where TAttributeValueFactory : class
    {
        /// <summary>
        /// Initializes this helper.
        /// </summary>
        void Initialize(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TAttributeValueFactory, TValue>> attrValueExpression);
    }
}