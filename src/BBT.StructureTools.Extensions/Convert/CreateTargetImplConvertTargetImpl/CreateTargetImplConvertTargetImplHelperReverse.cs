namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateTargetImplConvertTargetImplHelperReverse<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>
        : ICreateTargetImplConvertTargetImplHelper<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TReverseRelation : class
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTargetImpl, TTargetImpl> instanceCreator;
        private readonly IConvert<TSource, TTargetImpl, TConvertIntention> convert;

        private Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTargetImplConvertTargetImplHelperReverse{TSource,TTarget,TTargetImpl,TReverseRelation,TConvertIntention}" /> class.
        /// </summary>
        public CreateTargetImplConvertTargetImplHelperReverse(
            IInstanceCreator<TTargetImpl, TTargetImpl> instanceCreator,
            IConvert<TSource, TTargetImpl, TConvertIntention> convert)
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
        public void Convert(
            TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            this.convert.Convert(source, (TTargetImpl)target, additionalProcessings);
        }
    }
}