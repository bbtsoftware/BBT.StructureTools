namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class CopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild> : ICopyOperationCreateToManyWithReverseRelation<TParent, TChild, TConcreteChild>
        where TParent : class
        where TChild : class
        where TConcreteChild : class, TChild, new()
    {
        private ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper;
        private Func<TParent, IEnumerable<TChild>> sourceFunc;
        private Maybe<Expression<Func<TParent, ICollection<TChild>>>> maybeTargetExpression;

        /// <inheritdoc/>
        public void Initialize(
            Func<TParent, IEnumerable<TChild>> sourceFunc,
            Maybe<Expression<Func<TParent, ICollection<TChild>>>> maybeTargetExpression,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper)
        {
            sourceFunc.Should().NotBeNull();
            createCopyHelper.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.maybeTargetExpression = maybeTargetExpression;
            this.createCopyHelper = createCopyHelper;
        }

        /// <inheritdoc/>
        public void Copy(TParent source, TParent target, ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            var sourceValues = this.sourceFunc.Invoke(source)?.ToList();
            sourceValues.Should().NotBeNull();

            var copies = sourceValues.Select(sourceValue => this.createCopyHelper.CreateTarget(
                sourceValue as TConcreteChild,
                target,
                copyCallContext)).ToList();

            this.maybeTargetExpression.Do(targetExpression =>
            {
                target.AddRangeToCollectionFilterNulvalues(
                    targetExpression,
                    copies);
            });
        }
    }
}