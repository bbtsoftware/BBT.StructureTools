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
        private readonly IInstanceCreator<TChild, TConcreteChild> instancecreator;
        private readonly ICopy<TChild> copy;

        /// <summary>
        /// The <typeparamref name="TConcreteChild"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TChild, TParent>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TChild,TConcreteChild,TParent}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TChild, TConcreteChild> instancecreator,
            ICopy<TChild> copy)
        {
            instancecreator.Should().NotBeNull();
            copy.Should().NotBeNull();

            this.instancecreator = instancecreator;
            this.copy = copy;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        public void SetupReverseRelation(Expression<Func<TChild, TParent>> reverseRelationExpr)
        {
            reverseRelationExpr.Should().NotBeNull();

            this.reverseRelationExpr = reverseRelationExpr;
        }

        /// <summary>
        /// See <see cref="ICreateCopyHelper{TChild,TConcreteChild,TParent}.CreateTarget"/>.
        /// </summary>
        public TChild CreateTarget(
            TConcreteChild source,
            TParent reverseRelation,
            ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            reverseRelation.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            if (copyCallContext.AdditionalProcessings.OfType<IGenericContinueCopyInterception<TChild>>().Any(continueCopyInterception => !continueCopyInterception.Shalcopy(source)))
            {
                return null;
            }

            var target = this.instancecreator.Create();

            // Back reference should be set before copy method because copy could potentially use this back reference.
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            this.copy.Copy(source, target, copyCallContext);

            // Make sure that copy did not overwrite the target's reverse relation therefore back reference is set here
            // for a second time.
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            return target;
        }
    }

    /// <summary>
    /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMyOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    public class CreateCopyHelper<TTarget, TConcreteTarget>
        : ICreateCopyHelper<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        private readonly IInstanceCreator<TConcreteTarget, TConcreteTarget> instancecreator;
        private readonly ICopy<TTarget> copy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TTarget,TConcreteTarget}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TConcreteTarget, TConcreteTarget> instancecreator,
            ICopy<TTarget> copy)
        {
            instancecreator.Should().NotBeNull();
            copy.Should().NotBeNull();

            this.instancecreator = instancecreator;
            this.copy = copy;
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

            var target = this.instancecreator.Create();
            this.copy.Copy(source, target, additionalProcessings);
            return target;
        }
    }
}
