// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    /// <typeparam name="TReverseRelation">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class CreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TConcreteTarget> mInstanceCreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> mConvert;

        /// <summary>
        /// The <typeparamref name="TTarget"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TTarget, TReverseRelation>> mReverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public CreateConvertHelper(
            IInstanceCreator<TTarget, TConcreteTarget> aInstanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> aConvert)
        {
            aInstanceCreator.Should().NotBeNull();
            aConvert.Should().NotBeNull();

            this.mInstanceCreator = aInstanceCreator;
            this.mConvert = aConvert;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        public void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> aReverseRelationExpr)
        {
            aReverseRelationExpr.Should().NotBeNull();

            this.mReverseRelationExpr = aReverseRelationExpr;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.CreateTarget"/>.
        /// </summary>
        public TTarget CreateTarget(
            TSource source,
            TReverseRelation aReverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            aReverseRelation.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lTarget = this.mInstanceCreator.Create();
            lTarget.SetPropertyValue(this.mReverseRelationExpr, aReverseRelation);
            this.mConvert.Convert(source, lTarget, additionalProcessings);
            return lTarget;
        }
    }

    /// <summary>
    /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    public class CreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TConcreteTarget> mInstanceCreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> mConvert;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertHelper{TSource,TTarget,TConcreteTarget,TConvertIntention}" /> class.
        /// </summary>
        public CreateConvertHelper(
            IInstanceCreator<TTarget, TConcreteTarget> aInstanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> aConvert)
        {
            aInstanceCreator.Should().NotBeNull();
            aConvert.Should().NotBeNull();

            this.mInstanceCreator = aInstanceCreator;
            this.mConvert = aConvert;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.CreateTarget"/>.
        /// </summary>
        public TTarget CreateTarget(
            TSource source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var lTarget = this.mInstanceCreator.Create();
            this.mConvert.Convert(source, lTarget, additionalProcessings);
            return lTarget;
        }
    }
}
