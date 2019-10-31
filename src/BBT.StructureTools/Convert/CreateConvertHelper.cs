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
        private readonly IInstanceCreator<TTarget, TConcreteTarget> instancecreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> convert;

        /// <summary>
        /// The <typeparamref name="TTarget"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public CreateConvertHelper(
            IInstanceCreator<TTarget, TConcreteTarget> instanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> convert)
        {
            instanceCreator.Should().NotBeNull();
            convert.Should().NotBeNull();

            this.instancecreator = instanceCreator;
            this.convert = convert;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.SetupReverseRelation"/>.
        /// </summary>
        public void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
        {
            reverseRelationExpr.Should().NotBeNull();

            this.reverseRelationExpr = reverseRelationExpr;
        }

        /// <summary>
        /// See <see cref="ICreateConvertHelper{TSource,TTarget,TConcreteTarget,TReverseRelation,TConvertIntention}.CreateTarget"/>.
        /// </summary>
        public TTarget CreateTarget(
            TSource source,
            TReverseRelation reverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.Should().NotBeNull();
            reverseRelation.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            var target = this.instancecreator.Create();
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            this.convert.Convert(source, target, additionalProcessings);
            return target;
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
        "SA1402:FileMyOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    public class CreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TConcreteTarget> instancecreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertHelper{TSource,TTarget,TConcreteTarget,TConvertIntention}" /> class.
        /// </summary>
        public CreateConvertHelper(
            IInstanceCreator<TTarget, TConcreteTarget> instancecreator,
            IConvert<TSource, TTarget, TConvertIntention> convert)
        {
            instancecreator.Should().NotBeNull();
            convert.Should().NotBeNull();

            this.instancecreator = instancecreator;
            this.convert = convert;
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

            var target = this.instancecreator.Create();
            this.convert.Convert(source, target, additionalProcessings);
            return target;
        }
    }
}
