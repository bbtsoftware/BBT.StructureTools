namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild> : ICopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild>
        where TParent : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        private ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper;

        /// <summary>
        /// Function to get the source's property value.
        /// </summary>
        private Func<TParent, IEnumerable<TChild>> sourceFunc;

        /// <summary>
        ///  Expression which declares the target value.
        /// </summary>
        private Maybe<Expression<Func<TParent, ICollection<TChild>>>> maybeTargetExpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TParent, IEnumerable<TChild>> sourceFunc,
            Maybe<Expression<Func<TParent, ICollection<TChild>>>> maybeTargetExpression,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            createCopyHelper.NotNull(nameof(createCopyHelper));

            this.sourceFunc = sourceFunc;
            this.maybeTargetExpression = maybeTargetExpression;
            this.createCopyHelper = createCopyHelper;
        }

        /// <inheritdoc/>
        public void Copy(TParent source, TParent target, ICopyCallContext copyCallContext)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            copyCallContext.NotNull(nameof(copyCallContext));

            var sourceValues = this.sourceFunc.Invoke(source)?.ToList();
            sourceValues.NotNull(nameof(sourceValues));

            // ReSharper disable once AssignNullToNotNullAttribute -> StructureToolsArgumentChecks.NotNull is called before!
            var copies = sourceValues.Select(sourceValue => this.createCopyHelper.CreateTarget(
                sourceValue as TConcreteChild,
                target,
                copyCallContext)).ToList();

            this.maybeTargetExpression.Do(targetExpression =>
            {
                target.AddRangeFilterNullValues(
                    targetExpression,
                    copies);
            });
        }
    }
}