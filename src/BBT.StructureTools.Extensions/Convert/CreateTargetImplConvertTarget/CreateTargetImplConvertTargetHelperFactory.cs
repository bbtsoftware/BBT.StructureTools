namespace BBT.StructureTools.Extensions.Convert
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    public class CreateTargetImplConvertTargetHelperFactory<TSource, TTarget, TTargetImpl, TConvertIntention>
        : ICreateTargetImplConvertTargetHelperFactory<TSource, TTarget, TTargetImpl, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationFunc)
            where TReverseRelation : class
        {
            reverseRelationFunc.NotNull(nameof(reverseRelationFunc));

            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TReverseRelation, TConvertIntention>>();
            convertHelper.SetupReverseRelation(reverseRelationFunc);
            return convertHelper;
        }

        /// <inheritdoc/>
        public ICreateConvertHelper<TSource, TTarget, TConvertIntention> GetConvertHelper()
        {
            var convertHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateTargetImplConvertTargetHelper<TSource, TTarget, TTargetImpl, TConvertIntention>>();
            return convertHelper;
        }
    }
}
