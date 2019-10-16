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
        private readonly ICopyStrategyProvider<TStrategy, TChildType> mStrategyProvider;

        private Func<T, IEnumerable<TChildType>> mSourceFunc;
        private Expression<Func<T, ICollection<TChildType>>> mTargetExpression;
        private Func<TStrategy, TChildType> mCreateTargetChildFunc;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/> class.
        /// </summary>
        public CopyOperationCreateToManyWithGenericStrategy(ICopyStrategyProvider<TStrategy, TChildType> aGenericStrategyProvider)
        {
            aGenericStrategyProvider.Should().NotBeNull();

            this.mStrategyProvider = aGenericStrategyProvider;
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateToManyWithGenericStrategy{T,TStrategy,TChildType}"/>.
        /// </summary>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            var lNewKidsList = new List<TChildType>();

            foreach (var lChild in this.mSourceFunc.Invoke(source))
            {
                var lStrategy = this.mStrategyProvider.GetStrategy(lChild);
                var lChildCopy = this.mCreateTargetChildFunc.Invoke(lStrategy);
                lStrategy.Copy(lChild, lChildCopy, copyCallContext);
                lNewKidsList.Add(lChildCopy);
            }

            target.AddRangeToCollectionFilterNullValues(this.mTargetExpression, lNewKidsList);
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

            this.mSourceFunc = sourceFunc;
            this.mTargetExpression = targetExpression;
            this.mCreateTargetChildFunc = aCreateTargetChildExpression.Compile();
        }
    }
}