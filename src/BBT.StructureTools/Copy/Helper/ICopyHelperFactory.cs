namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Linq.Expressions;

    using BBT.StructureTools.Copy;

    /// <summary>
    /// Provides methods to create <see cref="ICreateCopyHelper{TChild,TConcreteChild,TParent}"/>.
    /// </summary>
    /// <typeparam name="TTarget">The target abstraction to convert to.</typeparam>
    /// <typeparam name="TConcreteTarget">The target to convert.</typeparam>
    public interface ICopyHelperFactory<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        /// <summary>
        /// Creates a <see cref="ICreateCopyHelper{TChild,TConcreteChild,TParent}"/>.
        /// </summary>
        /// <typeparam name="TReverseRelation">The <typeparamref name="TTarget"/>'s reverse relation.</typeparam>
        ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation> GetCopyHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class;

        /// <summary>
        /// Creates a <see cref="ICreateCopyHelper{TTarget, TConcreteTarget}"/>.
        /// </summary>
        ICreateCopyHelper<TTarget, TConcreteTarget> GetCopyHelper();
    }
}
