namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    public class CreateConvertFromStrategyHelperFactory<TSource, TTarget, TConvertIntention>
        : ICreateConvertFromStrategyHelperFactory<TSource, TTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConvertIntention : IBaseConvertIntention
    {
        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TConvertIntention> GetConvertHelper()
        {
            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateConvertFromStrategyHelper<TSource, TTarget, TConvertIntention>>();
            return convertHelper;
        }

        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class
        {
            reverseRelationExpr.NotNull(nameof(reverseRelationExpr));

            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateConvertFromStrategyHelper<TSource, TTarget, TReverseRelation, TConvertIntention>>();
            convertHelper.SetupReverseRelation(reverseRelationExpr);
            return convertHelper;
        }
    }
}