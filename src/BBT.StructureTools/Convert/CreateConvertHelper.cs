namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TConcreteTarget> instanceCreator;
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
            StructureToolsArgumentChecks.NotNull(instanceCreator, nameof(instanceCreator));
            StructureToolsArgumentChecks.NotNull(convert, nameof(convert));

            this.instanceCreator = instanceCreator;
            this.convert = convert;
        }

        /// <inheritdoc/>
        public void SetupReverseRelation(Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
        {
            StructureToolsArgumentChecks.NotNull(reverseRelationExpr, nameof(reverseRelationExpr));
            this.reverseRelationExpr = reverseRelationExpr;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(
            TSource source,
            TReverseRelation reverseRelation,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(reverseRelation, nameof(reverseRelation));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var target = this.instanceCreator.Create();
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            this.convert.Convert(source, target, additionalProcessings);
            return target;
        }
    }

    /// <inheritdoc/>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    internal class CreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        : ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTarget, TConcreteTarget> instanceCreator;
        private readonly IConvert<TSource, TTarget, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertHelper{TSource,TTarget,TConcreteTarget,TConvertIntention}" /> class.
        /// </summary>
        public CreateConvertHelper(
            IInstanceCreator<TTarget, TConcreteTarget> instanceCreator,
            IConvert<TSource, TTarget, TConvertIntention> convert)
        {
            StructureToolsArgumentChecks.NotNull(instanceCreator, nameof(instanceCreator));
            StructureToolsArgumentChecks.NotNull(convert, nameof(convert));

            this.instanceCreator = instanceCreator;
            this.convert = convert;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(
            TSource source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var target = this.instanceCreator.Create();
            this.convert.Convert(source, target, additionalProcessings);
            return target;
        }
    }
}
