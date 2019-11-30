namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType> : ICopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType>
        where T : class
        where TStrategy : class, ICopyStrategy<TChildType>
        where TChildType : class
    {
        private readonly ICopyStrategyProvider<TStrategy, TChildType> strategyProvider;

        private Func<T, IEnumerable<TChildType>> sourceFunc;
        private Func<T, ICollection<TChildType>> targetFunc;
        private Func<TStrategy, TChildType> createTargetChildFunc;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategy(ICopyStrategyProvider<TStrategy, TChildType> genericStrategyProvider)
        {
            genericStrategyProvider.NotNull(nameof(genericStrategyProvider));

            this.strategyProvider = genericStrategyProvider;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            var newKids = new List<TChildType>();

            foreach (var child in this.sourceFunc.Invoke(source))
            {
                var strategy = this.strategyProvider.GetStrategy(child);
                var childCopy = this.createTargetChildFunc.Invoke(strategy);
                strategy.Copy(child, childCopy, copyCallContext);
                newKids.Add(childCopy);
            }

            var targetCollection = this.targetFunc.Invoke(target);
            targetCollection.AddRangeToMe(newKids);
        }

        /// <inheritdoc/>
        public void Initialize(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> aCreateTargetChildExpression)
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));
            aCreateTargetChildExpression.NotNull(nameof(aCreateTargetChildExpression));

            this.sourceFunc = sourceFunc;
            this.targetFunc = targetExpression.Compile();
            this.createTargetChildFunc = aCreateTargetChildExpression.Compile();
        }
    }
}