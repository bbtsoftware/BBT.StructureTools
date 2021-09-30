namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    public class CreateTargetImplConvertTargetImplHelperFactory<TSource, TTarget, TTargetImpl, TConvertIntention>
        : ICreateTargetImplConvertTargetImplHelperFactory<TSource, TTarget, TTargetImpl, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TConvertIntention> GetConvertHelper()
        {
            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateTargetImplConvertTargetImplHelper<TSource, TTarget, TTargetImpl, TConvertIntention>>();
            return convertHelper;
        }

        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class
        {
            reverseRelationExpr.NotNull(nameof(reverseRelationExpr));

            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateTargetImplConvertTargetImplHelper<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>>();
            convertHelper.SetupReverseRelation(reverseRelationExpr);
            return convertHelper;
        }
    }
}