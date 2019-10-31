// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Implementation of <see cref="ICopyOperationCreateToOneWithReverseRelation{TParent,TChild,TConcreteChild}"/>.
    /// </summary>
    /// <typeparam name="T">See documentation on interface declaration.</typeparam>
    /// <typeparam name="TChild">See documentation on interface declaration.</typeparam>
    /// <typeparam name="TConcreteChild">See documentation on interface declaration.</typeparam>
    public class CopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild> : ICopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild>
        where T : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        private Func<T, TChild> sourceFunc;

        private Expression<Func<T, TChild>> targetexpression;

        private ICreateCopyHelper<TChild, TConcreteChild, T> createCopyHelper;

        /// <inheritdoc/>
        public void Initialize(
            Func<T, TChild> sourceFunc,
            Expression<Func<T, TChild>> targetFuncExpr,
            ICreateCopyHelper<TChild, TConcreteChild, T> aCreateCopyHelper)
        {
            aCreateCopyHelper.Should().NotBeNull();
            sourceFunc.Should().NotBeNull();
            targetFuncExpr.Should().NotBeNull();

            this.targetexpression = targetFuncExpr;
            this.sourceFunc = sourceFunc;
            this.createCopyHelper = aCreateCopyHelper;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            var sourceChild = this.sourceFunc.Invoke(source);

            // if the source is null, set the target also to null and exit copy process step.
            if (sourceChild == null)
            {
                target.SetPropertyValue(this.targetexpression, null);
                return;
            }

            var sourceConcrete = sourceChild as TConcreteChild;
            sourceConcrete.Should().NotBeNull();

            var copy = this.createCopyHelper.CreateTarget(
                sourceConcrete,
                target,
                copyCallContext);

            target.SetPropertyValue(this.targetexpression, copy);
        }
    }
}