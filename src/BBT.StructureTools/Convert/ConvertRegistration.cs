namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <inheritdoc/>
    internal class ConvertRegistration<TSource, TTarget> : IConvertRegistration<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private readonly ICollection<IConvertOperation<TSource, TTarget>> convertHelperOperationWorkUnits =
            new Collection<IConvertOperation<TSource, TTarget>>();

        private readonly IIocResolver serviceLocator = IocHandler.Instance.IocResolver;

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttribute<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator.GetInstance<IOperationCopyValue<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithMapping<TSourceValue, TTargetValue>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator.GetInstance<IOperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithLookUp<TValue>(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceLookUpFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(sourceLookUpFunc, nameof(sourceLookUpFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator.GetInstance<IOperationCopyValueWithLookUp<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, sourceLookUpFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithUpperLimit<TValue>(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
            where TValue : IComparable<TValue>
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(sourceUpperLimitFunc, nameof(sourceUpperLimitFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator.GetInstance<IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, sourceUpperLimitFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithSourceFilter<TValue>(
            Func<TSource, TTarget, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator.GetInstance<IOperationCopyValueWithSourceFilter<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfSourceNotDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation =
                this.serviceLocator.GetInstance<IOperationCopyValueIfSourceNotDefault<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfTargetIsDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation =
                this.serviceLocator.GetInstance<IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterSubConvert<TSourceValue, TConvertIntention>()
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationSourceSubConvert<TSource, TTarget, TSourceValue, TConvertIntention>>();
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterSubConvert<TSourceValue, TTargetValue, TConvertIntention>()
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>>();
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterTargetSubConvert<TTargetValue, TConvertIntention>()
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationTargetSubConvert<TSource, TTarget, TTargetValue, TConvertIntention>>();
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertToMany<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> filterFunc)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetFunc,
                filterFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertFromTargetOnDifferentLevels<TSourceValue, TConvertIntention>(
            Func<TTarget, TSourceValue> sourceExpression)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationConvertFromTargetOnDifferentLevels<TSource, TTarget, TSourceValue, TConvertIntention>>();
            operation.Initialize(sourceExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertFromSourceOnDifferentLevels<TSourceValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTarget, TConvertIntention>>();
            operation.Initialize(sourceFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertFromSourceOnDifferentLevels<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.serviceLocator
                .GetInstance<IOperationConvertFromSourceOnDifferentLevels<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>>();
            operation.Initialize(sourceFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateFromSourceWithReverseRelation<TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(targetExpression, createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, TBaseSource> baseSourceFunc,
            Expression<Func<TTarget, TBaseTarget>> targetValueExpression,
            Expression<Func<TBaseTarget, TTarget>> targetParentExpression)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(baseSourceFunc, nameof(baseSourceFunc));

            var operation = this.serviceLocator
                .GetInstance<IOperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>>();
            operation.Initialize(baseSourceFunc, targetValueExpression, targetParentExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToOneWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(relationFunc, nameof(relationFunc));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToOneWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                relationFunc,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOne<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToOne<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterSubCopy<TSubCopy>()
            where TSubCopy : class, ICopy<TSource>, ICopy<TTarget>
        {
            var copyType = typeof(TSubCopy).GenericTypeArguments[0];

            var copyOperationType = typeof(IOperationSubCopy<,,>).MakeGenericType(typeof(TSource), typeof(TTarget), copyType);

            var operation = (IConvertOperation<TSource, TTarget>)this.serviceLocator.GetInstance(copyOperationType);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterMergeLevel<TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>(
            Func<TSource, IEnumerable<TMergeValue>> mergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TMergeValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(mergeFunc, nameof(mergeFunc));
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(createConvertHelper, nameof(createConvertHelper));

            var operation =
                this.serviceLocator
                .GetInstance<IOperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>>();
            operation.Initialize(
                mergeFunc,
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyFromHist<TSourceValue, TTemporalDataType, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
            where TSourceValue : class, TTemporalDataType
            where TTemporalDataType : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));

            var operation = this.serviceLocator
                .GetInstance<IOperationCopyFromHist<TSource, TTarget, TSourceValue, TTemporalDataType, TConvertIntention>>();
            operation.Initialize(sourceFunc, referenceDateFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyFromMany<TSourceValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));

            var operation = this.serviceLocator.GetInstance<IOperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>>();
            operation.Initialize(sourceFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToManyWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToManyWithRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                relationFunc,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyGeneric<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToManyGeneric<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithSourceFilterAndReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToManyWithSourceFilterAndReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOneHistWithCondition<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TTemporalData, TConvertIntention>(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, bool> toOneHistCriteria,
            Func<TSource, TTarget, DateTime> toOneReferenceDate,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class, TTemporalData
            where TTargetValue : class, TTemporalData
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TTemporalData : class
            where TConvertIntention : IBaseConvertIntention
        {
            StructureToolsArgumentChecks.NotNull(sourceFunc, nameof(sourceFunc));
            StructureToolsArgumentChecks.NotNull(targetExpression, nameof(targetExpression));
            StructureToolsArgumentChecks.NotNull(toOneHistCriteria, nameof(toOneHistCriteria));
            StructureToolsArgumentChecks.NotNull(toOneReferenceDate, nameof(toOneReferenceDate));

            var operation = this.serviceLocator
                .GetInstance<IOperationCreateToOneHistWithCondition<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TTemporalData, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                toOneHistCriteria,
                toOneReferenceDate,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertOperations<TSource, TTarget> EndRegistrations()
        {
            return new ConvertOperations<TSource, TTarget>(
                this.convertHelperOperationWorkUnits);
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterPostProcessings(IConvertPostProcessing<TSource, TTarget> additionalProcessing, params IConvertPostProcessing<TSource, TTarget>[] furtherAdditionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(additionalProcessing, nameof(additionalProcessing));

            var list = new Collection<IBaseAdditionalProcessing>()
                            {
                                additionalProcessing,
                            };

            if (furtherAdditionalProcessings != null)
            {
                list.AddRangeToMe(furtherAdditionalProcessings);
            }

            var convertOpsPostProcessing = new OperationConvertPostProcessing<TSource, TTarget>(list);
            this.convertHelperOperationWorkUnits.Add(convertOpsPostProcessing);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> reverseRelationOnTarget)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
        {
            var operation = IocHandler.Instance.IocResolver.GetInstance<IOperationConditionalCreateToManyWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>>();
            operation.Initialize(
                source,
                targetParent,
                reverseRelationOnTarget);

            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }
    }
}
