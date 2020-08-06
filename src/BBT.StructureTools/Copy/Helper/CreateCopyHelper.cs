namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CreateCopyHelper<TChild, TConcreteChild, TParent> : ICreateCopyHelper<TChild, TConcreteChild, TParent>
        where TChild : class
        where TConcreteChild : class, TChild, new()
        where TParent : class
    {
        private readonly IInstanceCreator<TChild, TConcreteChild> instanceCreator;
        private readonly ICopy<TChild> copy;

        /// <summary>
        /// The <typeparamref name="TConcreteChild"/>'s reverse relation.
        /// </summary>
        private Expression<Func<TChild, TParent>> reverseRelationExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TChild,TConcreteChild,TParent}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TChild, TConcreteChild> instanceCreator,
            ICopy<TChild> copy)
        {
            StructureToolsArgumentChecks.NotNull(instanceCreator, nameof(instanceCreator));
            StructureToolsArgumentChecks.NotNull(copy, nameof(copy));

            this.instanceCreator = instanceCreator;
            this.copy = copy;
        }

        /// <inheritdoc/>
        public void SetupReverseRelation(Expression<Func<TChild, TParent>> reverseRelationExpr)
        {
            StructureToolsArgumentChecks.NotNull(reverseRelationExpr, nameof(reverseRelationExpr));
            this.reverseRelationExpr = reverseRelationExpr;
        }

        /// <inheritdoc/>
        public TChild CreateTarget(
            TConcreteChild source,
            TParent reverseRelation,
            ICopyCallContext copyCallContext)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(reverseRelation, nameof(reverseRelation));
            StructureToolsArgumentChecks.NotNull(copyCallContext, nameof(copyCallContext));

            if (copyCallContext.AdditionalProcessings.OfType<IGenericContinueCopyInterception<TChild>>().Any(continueCopyInterception => !continueCopyInterception.ShallCopy(source)))
            {
                return null;
            }

            var target = this.instanceCreator.Create();

            // Back reference should be set before copy method because copy could potentially use this back reference.
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            this.copy.Copy(source, target, copyCallContext);

            // Make sure that copy did not overwrite the target's reverse relation therefore back reference is set here
            // for a second time.
            target.SetPropertyValue(this.reverseRelationExpr, reverseRelation);
            return target;
        }
    }

    /// <inheritdoc/>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It is a variance of the same class with different number of generic parameters")]
    internal class CreateCopyHelper<TTarget, TConcreteTarget>
        : ICreateCopyHelper<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        private readonly IInstanceCreator<TConcreteTarget, TConcreteTarget> instanceCreator;
        private readonly ICopy<TTarget> copy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCopyHelper{TTarget,TConcreteTarget}"/> class.
        /// </summary>
        public CreateCopyHelper(
            IInstanceCreator<TConcreteTarget, TConcreteTarget> instanceCreator,
            ICopy<TTarget> copy)
        {
            StructureToolsArgumentChecks.NotNull(instanceCreator, nameof(instanceCreator));
            StructureToolsArgumentChecks.NotNull(copy, nameof(copy));

            this.instanceCreator = instanceCreator;
            this.copy = copy;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(
            TConcreteTarget source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(source, nameof(source));
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            var target = this.instanceCreator.Create();
            this.copy.Copy(source, target, additionalProcessings);
            return target;
        }
    }
}
