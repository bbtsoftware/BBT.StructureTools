namespace BBT.StructureTools.Initialization
{
    using System;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <summary>
    /// The IoC handler as singleton.
    /// </summary>
    public sealed class IocHandler
    {
        private static readonly Lazy<IocHandler> Lazy = new Lazy<IocHandler>(() => new IocHandler());

        /// <summary>
        /// Initializes a new instance of the <see cref="IocHandler"/> class.
        /// singleton pattern.
        /// </summary>
        private IocHandler()
        {
        }

        /// <summary>
        /// Gets singleton pattern.
        /// </summary>
        public static IocHandler Instance
        {
            get { return Lazy.Value; }
        }

        /// <summary>
        /// Gets or sets the object to be set for IoC handling.
        /// </summary>
        public IIocResolver IocResolver { get; set; }

        /// <summary>
        /// Executes the binding between interfaces and implementations within BBT.StructureTools
        /// using the <paramref name="singletonRegistrationAction"/> for each pair.
        /// </summary>
        /// <param name="singletonRegistrationAction">
        /// Action which is called to register an internal implementation for a given public abstraction as <c>singleton</c>.
        /// </param>
        /// <param name="transientRegistrationAction">
        /// Action which is called to register an internal implementation for a given public abstraction as <c>transiently</c>.
        /// </param>
        public void DoIocRegistrations(Action<Type, Type> singletonRegistrationAction, Action<Type, Type> transientRegistrationAction)
        {
            singletonRegistrationAction.NotNull(nameof(singletonRegistrationAction));
            transientRegistrationAction.NotNull(nameof(transientRegistrationAction));

            RegisterCompareTypes(singletonRegistrationAction);
            RegisterCopyTypes(singletonRegistrationAction, transientRegistrationAction);
            RegisterConvertTypes(singletonRegistrationAction, transientRegistrationAction);
            RegisterProviderTypes(singletonRegistrationAction);
        }

        /// <summary>
        /// Registers types needed for compare.
        /// </summary>
        private static void RegisterCompareTypes(Action<Type, Type> singletonRegistrationAction)
        {
            // Tools
            singletonRegistrationAction.Invoke(typeof(IComparer<,>), typeof(Comparer<,>));
            singletonRegistrationAction.Invoke(typeof(IEqualityComparerHelperRegistrationFactory), typeof(EqualityComparerHelperRegistrationFactory));

            // Helper
            singletonRegistrationAction.Invoke(typeof(ICompareHelper), typeof(CompareHelper));
        }

        /// <summary>
        /// Registers types needed for convert.
        /// </summary>
        private static void RegisterConvertTypes(Action<Type, Type> singletonRegistrationAction, Action<Type, Type> transientRegistrationAction)
        {
            // Tools
            singletonRegistrationAction.Invoke(typeof(IConvert<,,>), typeof(Converter<,,>));
            singletonRegistrationAction.Invoke(typeof(IConvertEngine<,>), typeof(ConvertEngine<,>));
            singletonRegistrationAction.Invoke(typeof(IConvertHelperFactory<,,,>), typeof(ConvertHelperFactory<,,,>));
            singletonRegistrationAction.Invoke(typeof(ICreateConvertHelper<,,,,>), typeof(CreateConvertHelper<,,,,>));
            singletonRegistrationAction.Invoke(typeof(ICreateConvertHelper<,,,>), typeof(CreateConvertHelper<,,,>));
            singletonRegistrationAction.Invoke(typeof(IConvertStrategyProvider<,,>), typeof(ConvertStrategyProvider<,,>));

            // Helper
            singletonRegistrationAction.Invoke(typeof(IConvertHelper), typeof(ConvertHelper));

            // Value conversion
            singletonRegistrationAction.Invoke(typeof(IConvertValue<,>), typeof(ValueConverter<,>));

            // Operations
            transientRegistrationAction.Invoke(typeof(IOperationCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationCreateFromSourceWithReverseRelation<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToManyWithReverseRelation<,,,,,,>), typeof(OperationCreateToManyWithReverseRelation<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToMany<,,,,,>), typeof(OperationCreateToMany<,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationMergeLevel<,,,,,,>), typeof(OperationMergeLevel<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationConvertFromSourceOnDifferentLevels<,,,,>), typeof(OperationConvertFromSourceOnDifferentLevels<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationConvertFromTargetOnDifferentLevels<,,,>), typeof(OperationConvertFromTargetOnDifferentLevels<,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationConvertToMany<,,,,>), typeof(OperationConvertToMany<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationSourceSubConvert<,,,>), typeof(OperationSourceSubConvert<,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationTargetSubConvert<,,,>), typeof(OperationTargetSubConvert<,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToOneWithReverseRelation<,,,,,>), typeof(OperationCreateToOneWithReverseRelation<,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToOne<,,,,,>), typeof(OperationCreateToOne<,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyFromMany<,,,>), typeof(OperationCopyFromMany<,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValue<,,>), typeof(OperationCopyValue<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueWithLookUp<,,>), typeof(OperationCopyValueWithLookUp<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueWithUpperLimit<,,>), typeof(OperationCopyValueWithUpperLimit<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueIfSourceNotDefault<,,>), typeof(OperationCopyValueIfSourceNotDefault<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueIfTargetIsDefault<,,>), typeof(OperationCopyValueIfTargetIsDefault<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueInclTargetArg<,,>), typeof(OperationCopyValueInclTargetArg<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationSubCopy<,,>), typeof(OperationSubCopy<,,>));
            transientRegistrationAction.Invoke(typeof(IOperationSubConvert<,,,,>), typeof(OperationSubConvert<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationConditionalCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationConditionalCreateFromSourceWithReverseRelation<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationConditionalCreateToManyWithReverseRelation<,,,,>), typeof(OperationConditionalCreateToManyWithReverseRelation<,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyValueWithMapping<,,,>), typeof(OperationCopyValueWithMapping<,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToOneHistWithCondition<,,,,,,,>), typeof(OperationCreateToOneHistWithCondition<,,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToManyWithRelation<,,,,,,>), typeof(OperationCreateToManyWithRelation<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToManyWithRelationInclTargetArg<,,,,,,>), typeof(OperationCreateToManyWithRelationInclTargetArg<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToOneWithRelation<,,,,,,>), typeof(OperationCreateToOneWithRelation<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCreateToOneWithRelationInclTargetArg<,,,,,,>), typeof(OperationCreateToOneWithRelationInclTargetArg<,,,,,,>));
            transientRegistrationAction.Invoke(typeof(IOperationCopyFromHist<,,,,>), typeof(OperationCopyFromHist<,,,,>));
        }

        /// <summary>
        /// Registers types needed for copy.
        /// </summary>
        private static void RegisterCopyTypes(Action<Type, Type> singletonRegistrationAction, Action<Type, Type> transientRegistrationAction)
        {
            // Tools
            singletonRegistrationAction.Invoke(typeof(ICreateCopyHelper<,>), typeof(CreateCopyHelper<,>));
            singletonRegistrationAction.Invoke(typeof(ICreateCopyHelper<,,>), typeof(CreateCopyHelper<,,>));
            singletonRegistrationAction.Invoke(typeof(ICopyHelperFactory<,>), typeof(CopyHelperFactory<,>));
            singletonRegistrationAction.Invoke(typeof(ICopy<>), typeof(Copier<>));
            singletonRegistrationAction.Invoke(typeof(ICopyHelperRegistrationFactory), typeof(CopyHelperRegistrationFactory));
            singletonRegistrationAction.Invoke(typeof(ICopyStrategyProvider<,>), typeof(GenericCopyStrategyProvider<,>));

            // Helper
            singletonRegistrationAction.Invoke(typeof(ICopyHelper), typeof(CopyHelper));

            // Operations
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToManyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithReverseRelation<,,>));
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToOneWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithReverseRelation<,,>));
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategy<,,>), typeof(CopyOperationCreateToManyWithGenericStrategy<,,>));
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>));
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>));
            transientRegistrationAction.Invoke(typeof(ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>));
        }

        /// <summary>
        /// Registers types needed for providers.
        /// </summary>
        private static void RegisterProviderTypes(Action<Type, Type> singletonRegistrationAction)
        {
            // Tools
            singletonRegistrationAction.Invoke(typeof(IDefaultValueProvider), typeof(DefaultValueProvider));
        }
    }
}
