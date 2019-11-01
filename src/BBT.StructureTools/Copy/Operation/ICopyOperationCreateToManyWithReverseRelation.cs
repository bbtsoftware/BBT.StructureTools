namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools.Copy;

    /// <summary>
    /// Strategy to copy entities with a <c>ToMany</c> relationship. <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="TParent">See link above.</typeparam>
    /// <typeparam name="TChild">See link above.</typeparam>
    /// <typeparam name="TConcreteChild">concrete implementation.</typeparam>
    public interface ICopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild>
        : ICopyOperation<TParent>
        where TParent : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        /// <summary>
        /// Initializes this helper.
        /// </summary>
        void Initialize(
            Func<TParent, IEnumerable<TChild>> sourceFunc,
            Maybe<Expression<Func<TParent, ICollection<TChild>>>> maybeTargetExpression,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper);
    }
}