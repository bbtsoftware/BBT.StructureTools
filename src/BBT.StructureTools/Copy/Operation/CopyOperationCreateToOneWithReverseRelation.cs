namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Implementation of
    /// <see cref="ICopyOperationCreateToOneWithReverseRelation{TParent,TChild,TConcreteChild}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// See documentation on interface declaration.
    /// </typeparam>
    /// <typeparam name="TChild">
    /// See documentation on interface declaration.
    /// </typeparam>
    /// <typeparam name="TConcreteChild">
    /// See documentation on interface declaration.
    /// </typeparam>
    internal class CopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild> : ICopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild>
        where T : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        private Func<T, TChild> sourceFunc;

        private Expression<Func<T, TChild>> targetExpression;

        private ICreateCopyHelper<TChild, TConcreteChild, T> createCopyHelper;

        /// <inheritdoc/>
        public void Initialize(
            Func<T, TChild> sourceFunc,
            Expression<Func<T, TChild>> targetFuncExpr,
            ICreateCopyHelper<TChild, TConcreteChild, T> createCopyHelper)
        {
            createCopyHelper.NotNull(nameof(createCopyHelper));
            sourceFunc.NotNull(nameof(sourceFunc));
            targetFuncExpr.NotNull(nameof(targetFuncExpr));

            this.targetExpression = targetFuncExpr;
            this.sourceFunc = sourceFunc;
            this.createCopyHelper = createCopyHelper;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            copyCallContext.NotNull(nameof(copyCallContext));

            var sourceChild = this.sourceFunc.Invoke(source);

            // if the source is null, set the target also to null and exit copy process step.
            if (sourceChild == null)
            {
                target.SetPropertyValue(this.targetExpression, null);
                return;
            }

            var sourceConcrete = sourceChild as TConcreteChild;
            sourceConcrete.NotNull(nameof(sourceConcrete));

            var copy = this.createCopyHelper.CreateTarget(
                sourceConcrete,
                target,
                copyCallContext);

            target.SetPropertyValue(this.targetExpression, copy);
        }
    }
}