// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Strategy to convert entities with a <c>ToMany</c> relationship.
    /// See <see cref="IConvertOperation{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TParent">
    /// See link above.
    /// </typeparam>
    /// <typeparam name="TChild">
    /// See link above.
    /// </typeparam>
    /// <typeparam name="TConcreteChild">
    /// Concrete implementation.
    /// </typeparam>
    public class CopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild> : ICopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild>
        where TParent : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        private ICreateCopyHelper<TChild, TConcreteChild, TParent> mCreateCopyHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TParent, IEnumerable<TChild>> mSourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Maybe<Expression<Func<TParent, ICollection<TChild>>>> mMaybeTargetExpression;

        /// <summary>
        /// See <see cref="ICopyOperationCreateToManyWithReverseRelation{TParent, TChild, TConcreteChild}.Initialize(Func{TParent, IEnumerable{TChild}}, Maybe{Expression{Func{TParent, ICollection{TChild}}}}, ICreateCopyHelper{TChild, TConcreteChild, TParent})"/>.
        /// </summary>
        public void Initialize(
            Func<TParent, IEnumerable<TChild>> sourceFunc,
            Maybe<Expression<Func<TParent, ICollection<TChild>>>> aMaybeTargetExpression,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> aCreateCopyHelper)
        {
            sourceFunc.Should().NotBeNull();
            aCreateCopyHelper.Should().NotBeNull();

            this.mSourceFunc = sourceFunc;
            this.mMaybeTargetExpression = aMaybeTargetExpression;
            this.mCreateCopyHelper = aCreateCopyHelper;
        }

        /// <inheritdoc/>
        public void Copy(TParent source, TParent target, ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            var lSourceValues = this.mSourceFunc.Invoke(source)?.ToList();
            lSourceValues.Should().NotBeNull();

            // ReSharper disable once AssignNullToNotNullAttribute -> Checks.AssertNotNothing is called before!
            var lCopies = lSourceValues.Select(sourceValue => this.mCreateCopyHelper.CreateTarget(
                sourceValue as TConcreteChild,
                target,
                copyCallContext)).ToList();

            this.mMaybeTargetExpression.Do(targetExpression =>
            {
                target.AddRangeToCollectionFilterNullValues(
                    targetExpression,
                    lCopies);
            });
        }
    }
}