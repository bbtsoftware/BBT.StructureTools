namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using BBT.MaybePattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <summary>
    /// Helper for the copy mechanism.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    internal class CopyHelperRegistration<T> : ICopyHelperRegistration<T>
        where T : class
    {
        private readonly List<ICopyOperation<T>> registeredStrategies;
        private readonly IIocResolver serviceLocator = IocHandler.Instance.IocResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelperRegistration{T}"/> class.
        /// </summary>
        public CopyHelperRegistration()
        {
            this.registeredStrategies = new List<ICopyOperation<T>>();
        }

        /// <summary>
        /// Register a copy attribute of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to compare. Must be a reference type.</typeparam>
        public ICopyHelperRegistration<T> RegisterAttribute<TValue>(Expression<Func<T, TValue>> expression)
        {
            this.registeredStrategies.Add(new CopyOperation<T, TValue>(expression));
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterInlineValueProcessing<TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TValue>> attrValueExpression)
        {
            this.registeredStrategies.Add(new CopyOperationInlineProcessValue<T, TValue>(targetExpression, attrValueExpression));
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategy<TStrategy, TChildType>(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> createTargetChildExpression)
            where TStrategy : class, ICopyStrategy<TChildType>
            where TChildType : class
        {
            var copyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategy<T, TStrategy, TChildType>>();
            copyStrategy.Initialize(sourceFunc, targetExpression, createTargetChildExpression);
            this.registeredStrategies.Add(copyStrategy);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterSubCopy<TSubCopyOfT>()
            where TSubCopyOfT : class, ICopy<T>
        {
            var copy = IocHandler.Instance.IocResolver.GetInstance<TSubCopyOfT>();
            this.registeredStrategies.Add(new CopyOperationSubCopy<T>(copy));
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelation<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<T, ICollection<TChild>>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            sourceFunc.NotNull(nameof(sourceFunc));
            targetExpression.NotNull(nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<ICopyOperationCreateToManyWithReverseRelation<T, TChild, TConcreteChild>>();
            var copyHelperFactory = this.serviceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            operation.Initialize(
                sourceFunc,
                Maybe.Some(targetExpression),
                copyHelperFactory.GetCopyHelper(reverseRelationFunc));

            this.registeredStrategies.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelationOnly<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            sourceFunc.NotNull(nameof(sourceFunc));

            var operation = this.serviceLocator
                .GetInstance<ICopyOperationCreateToManyWithReverseRelation<T, TChild, TConcreteChild>>();
            var copyHelperFactory = this.serviceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            operation.Initialize(
                sourceFunc,
                Maybe.None<Expression<Func<T, ICollection<TChild>>>>(),
                copyHelperFactory.GetCopyHelper(reverseRelationFunc));

            this.registeredStrategies.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T>
            RegisterCreateToOneWithReverseRelation<TChild, TConcreteChild>(
            Expression<Func<T, TChild>> targetFuncExpr,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new()
        {
            targetFuncExpr.NotNull(nameof(targetFuncExpr));

            var operation = this.serviceLocator
                .GetInstance<ICopyOperationCreateToOneWithReverseRelation<T, TChild, TConcreteChild>>();
            var copyHelperFactory = this.serviceLocator.GetInstance<ICopyHelperFactory<TChild, TConcreteChild>>();

            operation.Initialize(
                targetFuncExpr.Compile(),
                targetFuncExpr,
                copyHelperFactory.GetCopyHelper(reverseRelationFunc));

            this.registeredStrategies.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterPostProcessings(IBaseAdditionalProcessing additionalProcessing, params IBaseAdditionalProcessing[] furtherAdditionalProcessings)
        {
            additionalProcessing.NotNull(nameof(additionalProcessing));

            var list = new Collection<IBaseAdditionalProcessing>()
                            {
                                additionalProcessing,
                            };

            if (furtherAdditionalProcessings != null)
            {
                list.AddRangeToMe(furtherAdditionalProcessings);
            }

            var copyOperationPostProcessings = new CopyOperationPostProcessing<T>();
            copyOperationPostProcessings.Initialize(list);
            this.registeredStrategies.Add(copyOperationPostProcessings);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterPostProcessing<TPostProcessing>()
            where TPostProcessing : IBaseAdditionalProcessing
        {
            var postProcessing = IocHandler.Instance.IocResolver.GetInstance<TPostProcessing>();

            var list = new Collection<IBaseAdditionalProcessing>()
                            {
                                postProcessing,
                            };

            var copyOperationPostProcessings = new CopyOperationPostProcessing<T>();
            copyOperationPostProcessings.Initialize(list);
            this.registeredStrategies.Add(copyOperationPostProcessings);

            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCrossReferenceProcessing<TCrossReferencedModel, TReferencingModel>(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty)
            where TCrossReferencedModel : class
            where TReferencingModel : class
        {
            referencingProperty.NotNull(nameof(referencingProperty));

            var copyOperationCrossReferenceProcessing = new CopyOperationCrossReferenceProcessing<T, TCrossReferencedModel, TReferencingModel>();
            copyOperationCrossReferenceProcessing.Initialize(referencingProperty);
            this.registeredStrategies.Add(copyOperationCrossReferenceProcessing);
            return this;
        }

        /// <inheritdoc/>
        public ICopyOperation<T> EndRegistrations()
        {
            return new CopyHelperOperations<T>(this.registeredStrategies);
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T>
            RegisterCreateFromFactory<TAttributeValueFactory, TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TAttributeValueFactory, TValue>> attrValueExpression)
            where TAttributeValueFactory : class
        {
            targetExpression.NotNull(nameof(targetExpression));
            attrValueExpression.NotNull(nameof(attrValueExpression));

            var operation = new CopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>();
            operation.Initialize(targetExpression, attrValueExpression);
            this.registeredStrategies.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<T, ICollection<TChild>>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            var copyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>>();
            copyStrategy.Initialize(sourceFunc, targetExpression, reverseRelationExpression);
            this.registeredStrategies.Add(copyStrategy);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyReverseRelationOnly<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc, Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            var copyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<T, TStrategy, TChild>>();
            copyStrategy.Initialize(sourceFunc, reverseRelationExpression);
            this.registeredStrategies.Add(copyStrategy);
            return this;
        }

        /// <inheritdoc/>
        public ICopyHelperRegistration<T> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Expression<Func<T, TChild>> targetExpression, Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class
        {
            targetExpression.NotNull(nameof(targetExpression));
            var copyStrategy = IocHandler.Instance.IocResolver.GetInstance<ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<T, TStrategy, TChild>>();
            copyStrategy.Initialize(targetExpression.Compile(), targetExpression, reverseRelationExpression);
            this.registeredStrategies.Add(copyStrategy);
            return this;
        }
    }
}