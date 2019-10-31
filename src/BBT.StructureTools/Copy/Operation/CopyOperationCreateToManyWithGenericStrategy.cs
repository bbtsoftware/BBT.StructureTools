// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// <see cref="ICopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/>.
    /// </summary>
    /// <typeparam name="T">see above.</typeparam>
    /// <typeparam name="TStrategy">see above.</typeparam>
    /// <typeparam name="TChildType">see above.</typeparam>
    public class CopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType> : ICopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType>
        where T : class
        where TStrategy : class, ICopyStrategy<TChildType>
        where TChildType : class
    {
        private readonly ICopyStrategyProvider<TStrategy, TChildType> strategyProvider;

        private Func<T, IEnumerable<TChildType>> sourceFunc;
        private Expression<Func<T, ICollection<TChildType>>> targetexpression;
        private Func<TStrategy, TChildType> createTargetChildFunc;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategy(ICopyStrategyProvider<TStrategy, TChildType> genericStrategyProvider)
        {
            genericStrategyProvider.Should().NotBeNull();

            this.strategyProvider = genericStrategyProvider;
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/>.
        /// </summary>
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

            target.AddRangeToCollectionFilterNulvalues(this.targetexpression, newKids);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/>.
        /// </summary>
        public void Initialize(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> aCreateTargetChildExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            aCreateTargetChildExpression.Should().NotBeNull();

            this.sourceFunc = sourceFunc;
            this.targetexpression = targetExpression;
            this.createTargetChildFunc = aCreateTargetChildExpression.Compile();
        }
    }
}