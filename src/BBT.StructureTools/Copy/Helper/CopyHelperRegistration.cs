// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <summary>
    /// Helper for the copy mechanism.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    public class CopyHelperRegistration<T> : ICopyHelperRegistration<T>
        where T : class
    {
        private readonly List<ICopyOperation<T>> mRegisteredStrategies;
        private readonly IIocResolver mServiceLocator = IocHandler.Instance.IocResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelperRegistration{T}"/> class.
        /// </summary>
        public CopyHelperRegistration()
        {
            this.mRegisteredStrategies = new List<ICopyOperation<T>>();
        }

        /// <summary>
        /// Register a copy attribute of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to compare. Must be a reference type.</typeparam>
        public ICopyHelperRegistration<T> RegisterAttribute<TValue>(Expression<Func<T, TValue>> aExpression)
        {
            this.mRegisteredStrategies.Add(new CopyOperation<T, TValue>(aExpression));
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyRegistrations{T}"/>.
        /// </summary>
        /// <typeparam name="TValue">see above.</typeparam>
        public ICopyHelperRegistration<T> RegisterInlineValueProcessing<TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TValue>> aAttrValueExpression)
        {
            this.mRegisteredStrategies.Add(new CopyOperationInlineProcessValue<T, TValue>(targetExpression, aAttrValueExpression));
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateToManyFromGenericStrategy{TStrategy, TChildType}"/>.
        /// </summary>
        /// <typeparam name="TStrategy">see above.</typeparam>
        /// <typeparam name="TChildType">see above.</typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategy<TStrategy, TChildType>(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> aCreateTargetChildExpression)
            where TStrategy : class, ICopyStrategy<TChildType>
            where TChildType : class
        {
            var lCopyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType>>();
            lCopyStrategy.Initialize(sourceFunc, targetExpression, aCreateTargetChildExpression);
            this.mRegisteredStrategies.Add(lCopyStrategy);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterSubCopy{TTypeOfSubCopy}"/>.
        /// </summary>
        /// <typeparam name="TSubCopyOfT">
        /// See link above.
        /// </typeparam>
        public ICopyHelperRegistration<T> RegisterSubCopy<TSubCopyOfT>()
            where TSubCopyOfT : class, ICopy<T>
        {
            var lCopy = IocHandler.Instance.IocResolver.GetInstance<TSubCopyOfT>();
            this.mRegisteredStrategies.Add(new CopyOperationSubCopy<T>(lCopy));
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateToManyWithReverseRelation{TChild, TConcreteChild}"/>.
        /// </summary>
        /// <typeparam name="TChild">See link above.</typeparam>
        /// <typeparam name="TConcreteChild">See link above.</typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelation<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<T, ICollection<TChild>>> targetExpression,
            Expression<Func<TChild, T>> aReverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var lOperation = this.mServiceLocator.GetInstance<ICopyOperationCreateToManyWithReverseRelation<T, TChild, TConcreteChild>>();
            var lCopyHelperFactory = this.mServiceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            lOperation.Initialize(
                sourceFunc,
                Maybe.Some(targetExpression),
                lCopyHelperFactory.GetCopyHelper(aReverseRelationFunc));

            this.mRegisteredStrategies.Add(lOperation);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateToManyWithReverseRelationOnly{TChild, TConcreteChild}"/>.
        /// </summary>
        /// <typeparam name="TChild">See link above.</typeparam>
        /// <typeparam name="TConcreteChild">See link above.</typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelationOnly<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> aReverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            sourceFunc.Should().NotBeNull();

            var lOperation = this.mServiceLocator
                .GetInstance<ICopyOperationCreateToManyWithReverseRelation<T, TChild, TConcreteChild>>();
            var lCopyHelperFactory = this.mServiceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            lOperation.Initialize(
                sourceFunc,
                Maybe.None<Expression<Func<T, ICollection<TChild>>>>(),
                lCopyHelperFactory.GetCopyHelper(aReverseRelationFunc));

            this.mRegisteredStrategies.Add(lOperation);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateToOneWithReverseRelation{TChild,TConcreteChild}"/>.
        /// </summary>
        /// <typeparam name="TChild">see above.</typeparam>
        /// <typeparam name="TConcreteChild">see above.</typeparam>
        public ICopyHelperRegistration<T>
            RegisterCreateToOneWithReverseRelation<TChild, TConcreteChild>(
            Expression<Func<T, TChild>> targetFuncExpr,
            Expression<Func<TChild, T>> aReverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            targetFuncExpr.Should().NotBeNull();

            var lOperation = this.mServiceLocator.GetInstance<ICopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild>>();
            var lCopyHelperFactory = this.mServiceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            lOperation.Initialize(
                targetFuncExpr.Compile(),
                targetFuncExpr,
                lCopyHelperFactory.GetCopyHelper(aReverseRelationFunc));

            this.mRegisteredStrategies.Add(lOperation);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterPostProcessings"/>.
        /// </summary>
        public ICopyHelperRegistration<T> RegisterPostProcessings(IBaseAdditionalProcessing aAdditionalProcessing, params IBaseAdditionalProcessing[] aFurtherAdditionalProcessings)
        {
            aAdditionalProcessing.Should().NotBeNull();

            var lList = new Collection<IBaseAdditionalProcessing>() { aAdditionalProcessing };

            if (aFurtherAdditionalProcessings != null)
            {
                lList.AddRangeToMe(aFurtherAdditionalProcessings);
            }

            var lCopyOperationPostProcessings = new CopyOperationPostProcessing<T>();
            lCopyOperationPostProcessings.Initialize(lList);
            this.mRegisteredStrategies.Add(lCopyOperationPostProcessings);
            return this;
        }

        /// <summary>
        /// <see cref="ICopyHelperRegistration{T}.RegisterPostProcessing{TPostProcessing}"/>.
        /// </summary>
        /// <typeparam name="TPostProcessing">see above.</typeparam>
        public ICopyHelperRegistration<T> RegisterPostProcessing<TPostProcessing>()
            where TPostProcessing : IBaseAdditionalProcessing
        {
            var lPostProcessing = IocHandler.Instance.IocResolver.GetInstance<TPostProcessing>();

            var lList = new Collection<IBaseAdditionalProcessing>() { lPostProcessing };

            var lCopyOperationPostProcessings = new CopyOperationPostProcessing<T>();
            lCopyOperationPostProcessings.Initialize(lList);
            this.mRegisteredStrategies.Add(lCopyOperationPostProcessings);

            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCrossReferenceProcessing{TCrossReferencedModel, TReferencingModel}"/>.
        /// </summary>
        /// <typeparam name="TCrossReferencedModel">See link above.</typeparam>
        /// <typeparam name="TReferencingModel">See link above.</typeparam>
        public ICopyHelperRegistration<T> RegisterCrossReferenceProcessing<TCrossReferencedModel, TReferencingModel>(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> aReferencingProperty)
            where TCrossReferencedModel : class
            where TReferencingModel : class
        {
            aReferencingProperty.Should().NotBeNull();

            var lCopyOperationCrossReferenceProcessing = new CopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel>();
            lCopyOperationCrossReferenceProcessing.Initialize(aReferencingProperty);
            this.mRegisteredStrategies.Add(lCopyOperationCrossReferenceProcessing);
            return this;
        }

        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        public ICopyOperation<T> EndRegistrations()
        {
            return new CopyHelperOperations<T>(this.mRegisteredStrategies);
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateFromFactory{TAttributeValueFactory,TValue}"/>.
        /// </summary>
        /// <typeparam name="TAttributeValueFactory">see above.</typeparam>
        /// <typeparam name="TValue">see above.</typeparam>
        public ICopyHelperRegistration<T>
            RegisterCreateFromFactory<TAttributeValueFactory, TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TAttributeValueFactory, TValue>> aAttrValueExpression)
            where TAttributeValueFactory : class
        {
            targetExpression.Should().NotBeNull();

            aAttrValueExpression.Should().NotBeNull();

            var lOperation = new CopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>();
            lOperation.Initialize(targetExpression, aAttrValueExpression);
            this.mRegisteredStrategies.Add(lOperation);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}"/>.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc, Expression<Func<T, ICollection<TChild>>> targetExpression, Expression<Func<TChild, T>> aReverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            var lCopyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>>();
            lCopyStrategy.Initialize(sourceFunc, targetExpression, aReverseRelationExpression);
            this.mRegisteredStrategies.Add(lCopyStrategy);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}.RegisterCreateToManyFromGenericStrategyReverseRelationOnly{TStrategy, TChild}(Func{T, IEnumerable{TChild}}, Expression{Func{TChild, T}})"/>.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyReverseRelationOnly<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc, Expression<Func<TChild, T>> aReverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            var lCopyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<T, TStrategy, TChild>>();
            lCopyStrategy.Initialize(sourceFunc, aReverseRelationExpression);
            this.mRegisteredStrategies.Add(lCopyStrategy);
            return this;
        }

        /// <summary>
        /// See <see cref="ICopyHelperRegistration{T}"/>.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        public ICopyHelperRegistration<T> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Expression<Func<T, TChild>> targetExpression, Expression<Func<TChild, T>> aReverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            targetExpression.Should().NotBeNull();

            var lCopyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>>();
            lCopyStrategy.Initialize(targetExpression.Compile(), targetExpression, aReverseRelationExpression);
            this.mRegisteredStrategies.Add(lCopyStrategy);
            return this;
        }
    }
}