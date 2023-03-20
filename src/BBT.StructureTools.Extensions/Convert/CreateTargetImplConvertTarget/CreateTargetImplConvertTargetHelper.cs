﻿namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>
        : ICreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TTargetImpl> instanceCreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> convert;

        /// <summary>
        /// The <typeparamref name="TTarget"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTargetImplConvertTargetHelper{TSource,TTarget,TTargetImpl,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public CreateTargetImplConvertTargetHelper(
            IInstanceCreator<TTarget, TTargetImpl> instanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> convert)
        {
            instanceCreator.NotNull(nameof(instanceCreator));
            convert.NotNull(nameof(convert));

            this.instanceCreator = instanceCreator;
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
        {
            reverseRelationExpr.NotNull(nameof(reverseRelationExpr));
            this.reverseRelationExpr = reverseRelationExpr;
        }

        /// <inheritdoc/>
        public TTarget Create(TSource source, TReverseRelation reverseRelation)
        {
            var target = this.instanceCreator.Create();
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            return target;
        }

        /// <inheritdoc/>
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            this.convert.Convert(source, target, additionalProcessings);
        }
    }

    /// <inheritdoc/>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    public class CreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TConvertIntention>
        : ICreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TTargetImpl> instanceCreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTargetImplConvertTargetHelper{TSource,TTarget,TTargetImpl,TConvertIntention}" /> class.
        /// </summary>
        public CreateTargetImplConvertTargetHelper(
            IInstanceCreator<TTarget, TTargetImpl> instanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> convert)
        {
            instanceCreator.NotNull(nameof(instanceCreator));
            convert.NotNull(nameof(convert));

            this.instanceCreator = instanceCreator;
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            this.convert.Convert(source, target, additionalProcessings);
        }

        /// <inheritdoc/>
        public TTarget Create(TSource source)
        {
            var target = this.instanceCreator.Create();
            return target;
        }
    }
}
