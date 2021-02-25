namespace BBT.StructureTools.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides methods to support copy.
    /// TODO ber: one type declaration per file.
    /// </summary>
    /// <typeparam name="TChild">The target interface to convert to.</typeparam>
    /// <typeparam name="TConcreteChild">
    /// The target to convert to.
    /// </typeparam>
    /// <typeparam name="TParent">
    /// Reverse relation type.
    /// </typeparam>
    internal interface ICreateCopyHelper<TChild, TConcreteChild, TParent>
        where TChild : class
        where TConcreteChild : class, TChild, new()
        where TParent : class
    {
        /// <summary>
        /// Setups the given reverse relation with the expression.
        /// </summary>
        void SetupReverseRelation(Expression<Func<TChild, TParent>> reverseRelationExpr);

        /// <summary>
        /// Creates a new <typeparamref name="TConcreteChild"/> converted from <paramref name="source"/>.
        /// </summary>
        TChild CreateTarget(
            TConcreteChild source,
            TParent reverseRelation,
            ICopyCallContext copyCallContext);
    }

    /// <summary>
    /// Provides methods to support copy.
    /// TODO ber: one type declaration per file.
    /// </summary>
    /// <typeparam name="TTarget">The target abstraction to convert to.</typeparam>
    /// <typeparam name="TConcreteTarget">The target to convert to.</typeparam>
    internal interface ICreateCopyHelper<out TTarget, in TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        /// <summary>
        /// Creates a new <typeparamref name="TConcreteTarget"/> copied from <paramref name="source"/>.
        /// </summary>
        TTarget CreateTarget(
            TConcreteTarget source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings);
    }
}
