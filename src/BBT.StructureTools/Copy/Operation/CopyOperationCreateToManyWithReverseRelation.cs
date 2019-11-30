﻿namespace BBT.StructureTools.Copy.Operation
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
        private Func<TParent, IEnumerable<TChild>> sourceFunc;
        private Maybe<Func<TParent, ICollection<TChild>>> maybeTargetFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationCreateToManyWithReverseRelation{TParent, TChild, TConcreteChild}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public CopyOperationCreateToManyWithReverseRelation()
        {
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<TParent, IEnumerable<TChild>> sourceFunc,
            Maybe<Func<TParent, ICollection<TChild>>> maybeTargetFunc,
            ICreateCopyHelper<TChild, TConcreteChild, TParent> createCopyHelper)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            createCopyHelper.NotNull(nameof(createCopyHelper));

            this.sourceFunc = sourceFunc;
            this.maybeTargetFunc = maybeTargetFunc;
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

            var copies = sourceValues.Select(sourceValue => this.createCopyHelper.CreateTarget(
                sourceValue as TConcreteChild,
                target,
                copyCallContext)).ToList();

            this.maybeTargetFunc.Do(targetFunc =>
            {
                var targetCollection = targetFunc.Invoke(target);
                foreach (var maybeCopy in copies)
                {
                    maybeCopy.Do(child => targetCollection.Add(child));
                }
            });
        }
    }
}