namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;

    /// <summary>
    /// Strategy to copy entities with a <c>ToMany</c> relationship.
    /// <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="TParent">See link above.</typeparam>
    /// <typeparam name="TChild">See link above.</typeparam>
    /// <typeparam name="TConcreteChild">concrete implementation.</typeparam>
    public interface ICopyOperationCreateToOneWithReverseRelation<TParent, TChild, TConcreteChild>
        : ICopyOperation<TParent>
        where TParent : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        /// <summary>
        /// Initializes this helper.
        /// </summary>
        /// <param name="sourceFunc">
        ///     The a Source Function.
        /// </param>
        /// <param name="targetFuncExpr">
        ///     The a Target Function Expression.
        /// </param>
        /// <param name="createCopyHelper">
        ///     <see cref="ICreateCopyHelper{TConcreteChild,TParent}"/> which can be used to create the
        ///     copied child instance.
        /// </param>
        void Initialize(
            Func<TParent, TChild> sourceFunc,
            Expression<Func<TParent, TChild>> targetFuncExpr,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper);
    }
}