namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    internal class CopyHelperFactory<TTarget, TConcreteTarget>
        : ICopyHelperFactory<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        /// <inheritdoc/>
        public ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation> GetCopyHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationExpr)
            where TReverseRelation : class
        {
            reverseRelationExpr.NotNull(nameof(reverseRelationExpr));

            var copyHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation>>();
            copyHelper.SetupReverseRelation(reverseRelationExpr);
            return copyHelper;
        }

        /// <inheritdoc/>
        public ICreateCopyHelper<TTarget, TConcreteTarget> GetCopyHelper()
        {
            var copyHelper = IocHandler.Instance.IocResolver
                .GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget>>();
            return copyHelper;
        }
    }
}
