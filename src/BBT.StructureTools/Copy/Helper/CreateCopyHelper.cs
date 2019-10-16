// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICreateCopyHelper{TChild,TConcreteChild,TParent}"/>.
    /// </summary>
    /// <typeparam name="TChild">See link above.</typeparam>
    /// <typeparam name="TConcreteChild">See link above.</typeparam>
    /// <typeparam name="TParent">See link above.</typeparam>
    public class CreateCopyHelper<TChild, TConcreteChild, TParent> : ICreateCopyHelper<TChild, TConcreteChild, TParent>
        where TChild : class
        where TConcreteChild : class, TChild, new()
        where TParent : class
    {
        private readonly IInstanceCreator<TChild, TConcreteChild> mInstanceCreator;
        private readonly ICopy<TChild> mCopy;

        /// <summary>
        /// The <typeparamref name="TConcreteChild"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TChild, TParent>> mReverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TChild,TConcreteChild,TParent}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TChild, TConcreteChild> aInstanceCreator,
            ICopy<TChild> aCopy)
        {
            aInstanceCreator.Should().NotBeNull();
            aCopy.Should().NotBeNull();

            this.mInstanceCreator = aInstanceCreator;
            this.mCopy = aCopy;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        public void SetupReverseRelation(Expression<Func<TChild, TParent>> aReverseRelationExpr)
        {
            aReverseRelationExpr.Should().NotBeNull();

            this.mReverseRelationExpr = aReverseRelationExpr;
        }

        /// <summary>
        /// See <see cref="ICreateCopyHelper{TChild,TConcreteChild,TParent}.CreateTarget"/>.
        /// </summary>
        public TChild CreateTarget(
            TConcreteChild source,
            TParent aReverseRelation,
            ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            aReverseRelation.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            if (copyCallContext.AdditionalProcessings.OfType<IGenericContinueCopyInterception<TChild>>().Any(aContinueCopyInterception => !aContinueCopyInterception.ShallCopy(source)))
            {
                return null;
            }

            var lTarget = this.mInstanceCreator.Create();

            // Back reference should be set before copy method because copy could potentially use this back reference.
            lTarget.SetPropertyValue(this.mReverseRelationExpr, aReverseRelation);
            this.mCopy.Copy(source, lTarget, copyCallContext);

            // Make sure that copy did not overwrite the target's reverse relation therefore back reference is set here
            // for a second time.
            lTarget.SetPropertyValue(this.mReverseRelationExpr, aReverseRelation);
            return lTarget;
        }
    }

    /// <summary>
    /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    public class CreateCopyHelper<TTarget, TConcreteTarget>
        : ICreateCopyHelper<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        private readonly IInstanceCreator<TConcreteTarget, TConcreteTarget> mInstanceCreator;
        private readonly ICopy<TTarget> mCopy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TTarget,TConcreteTarget}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TConcreteTarget, TConcreteTarget> aInstanceCreator,
            ICopy<TTarget> aCopy)
        {
            aInstanceCreator.Should().NotBeNull();
            aCopy.Should().NotBeNull();

            this.mInstanceCreator = aInstanceCreator;
            this.mCopy = aCopy;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.CreateTarget"/>.
        /// </summary>
        public TTarget CreateTarget(
            TConcreteTarget source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lTarget = this.mInstanceCreator.Create();
            this.mCopy.Copy(source, lTarget, additionalProcessings);
            return lTarget;
        }
    }
}
