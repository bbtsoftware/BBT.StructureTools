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
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelperFactory{TTarget, TConcreteTarget}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public CopyHelperFactory()
        {
        }

        /// <inheritdoc/>
        public ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation> GetCopyHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> reverseRelationFunc)
            where TReverseRelation : class
        {
            reverseRelationFunc.NotNull(nameof(reverseRelationFunc));

            var copyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation>>();
            copyHelper.SetupReverseRelation(reverseRelationFunc);
            return copyHelper;
        }

        /// <inheritdoc/>
        public ICreateCopyHelper<TTarget, TConcreteTarget> GetCopyHelper()
        {
            var copyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget>>();
            return copyHelper;
        }
    }
}
