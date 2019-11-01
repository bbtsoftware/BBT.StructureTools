namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <inheritdoc/>
    public class ConvertRegistration<TSource, TTarget> : IConvertRegistration<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private readonly ICollection<IConvertOperation<TSource, TTarget>> convertHelperOperationWorkUnits = new Collection<IConvertOperation<TSource, TTarget>>();
        private readonly IIocResolver servicelocator = IocHandler.Instance.IocResolver;

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopySource(
            Expression<Func<TTarget, TSource>> targetExpression)
        {
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopySource<TSource, TTarget>>();
            operation.Initialize(targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttribute<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyValue<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithMapping<TSourceValue, TTargetValue>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyValueWithMapping<TSource, TTarget, TSourceValue, TTargetValue>>();
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
            sourceFunc.Should().NotBeNull();
            sourceLookUpFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyValueWithLookUp<TSource, TTarget, TValue>>();
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
            sourceFunc.Should().NotBeNull();
            sourceUpperLimitFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyValueWithUpperLimit<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, sourceUpperLimitFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithSourceFilter<TValue>(
            Func<TSource, TTarget, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyValueWithSourceFilter<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfSourceNotDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation =
                this.servicelocator.GetInstance<IOperationCopyValueIfSourceNotDefault<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfTargetIsDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation =
                this.servicelocator.GetInstance<IOperationCopyValueIfTargetIsDefault<TSource, TTarget, TValue>>();
            operation.Initialize(sourceFunc, targetExpression);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterSubConvert<TSourceValue, TConvertIntention>()
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.servicelocator
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
            var operation = this.servicelocator
                .GetInstance<IOperationSubConvert<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>>();
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterTargetSubConvert<TTargetValue, TConvertIntention>()
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.servicelocator
                .GetInstance<IOperationTargetSubConvert<TSource, TTarget, TTargetValue, TConvertIntention>>();
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertToMany<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> aFilterFunc)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.servicelocator
                .GetInstance<IOperationConvertToMany<TSource, TTarget, TSourceValue, TTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetFunc,
                aFilterFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterConvertFromTargetOnDifferentLevels<TSourceValue, TConvertIntention>(
            Func<TTarget, TSourceValue> sourceExpression)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.servicelocator
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
            var operation = this.servicelocator
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
            var operation = this.servicelocator
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
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator
                .GetInstance<IOperationCreateFromSourceWithReverseRelation<TSource, TTarget, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(targetExpression, createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, TBaseSource> aBaseSourceFunc,
            Expression<Func<TTarget, TBaseTarget>> targetValueExpression,
            Expression<Func<TBaseTarget, TTarget>> targetParentExpression)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention
        {
            aBaseSourceFunc.Should().NotBeNull();

            var operation = this.servicelocator
                .GetInstance<IOperationConditionalCreateFromSourceWithReverseRelation<TSource, TTarget, TBaseSource, TBaseTarget, TIntention>>();
            operation.Initialize(aBaseSourceFunc, targetValueExpression, targetParentExpression);
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
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator
                .GetInstance<IOperationCreateToOneWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
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
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCreateToOne<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>>();
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

            var operation = (IConvertOperation<TSource, TTarget>)this.servicelocator.GetInstance(copyOperationType);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterMergeLevel<TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>(
            Func<TSource, IEnumerable<TMergeValue>> aMergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TMergeValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            aMergeFunc.Should().NotBeNull();
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();
            createConvertHelper.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationMergeLevel<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>>();
            operation.Initialize(
                aMergeFunc,
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyFromMany<TSourceValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            sourceFunc.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyFromMany<TSource, TTarget, TSourceValue, TConvertIntention>>();
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
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCreateToManyWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator.GetInstance<IOperationCopyOneToManyWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyGenericWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator
                .GetInstance<IOperationCreateToManyGenericWithReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
                createConvertHelper);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCreateToManyGeneric<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention
        {
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator
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
            sourceFunc.Should().NotBeNull();
            targetExpression.Should().NotBeNull();

            var operation = this.servicelocator
                .GetInstance<IOperationCreateToManyWithSourceFilterAndReverseRelation<TSource, TTarget, TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>>();
            operation.Initialize(
                sourceFunc,
                targetExpression,
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
        public IConvertRegistration<TSource, TTarget> RegisterPostProcessings(IConvertPostProcessing<TSource, TTarget> additionalProcessing, params IConvertPostProcessing<TSource, TTarget>[] aFurtherAdditionalProcessings)
        {
            additionalProcessing.Should().NotBeNull();

            var list = new Collection<IBaseAdditionalProcessing>() { additionalProcessing };

            if (aFurtherAdditionalProcessings != null)
            {
                list.AddRangeToMe(aFurtherAdditionalProcessings);
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

        /// <inheritdoc/>
        public IConvertRegistration<TSource, TTarget> RegisterCopyFromTemporalData<TSourceValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention
        {
            var operation = this.servicelocator
                   .GetInstance<IOperationCopyFromTemporalData<TSource, TTarget, TSourceValue, TConvertIntention>>();
            operation.Initialize(sourceFunc, referenceDateFunc);
            this.convertHelperOperationWorkUnits.Add(operation);
            return this;
        }
    }
}
