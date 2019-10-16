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
        /// using the <paramref name="registrationAction"/> for each pair.
        /// </summary>
        public void DoIocRegistrations(Action<Type, Type> registrationAction)
        {
            RegisterCompareTypes(registrationAction);
            RegisterCopyTypes(registrationAction);
            RegisterConvertTypes(registrationAction);
        }

        /// <summary>
        /// Registers types needed for compare.
        /// </summary>
        private static void RegisterCompareTypes(Action<Type, Type> registrationAction)
        {
            // Tools
            registrationAction.Invoke(typeof(IComparer<,>), typeof(Comparer<,>));
            registrationAction.Invoke(typeof(IEqualityComparerHelperRegistrationFactory), typeof(EqualityComparerHelperRegistrationFactory));

            // Helper
            registrationAction.Invoke(typeof(ICompareHelper), typeof(CompareHelper));
        }

        /// <summary>
        /// Registers types needed for convert.
        /// </summary>
        private static void RegisterConvertTypes(Action<Type, Type> registrationAction)
        {
            // Tools
            registrationAction.Invoke(typeof(IConvert<,,>), typeof(Converter<,,>));
            registrationAction.Invoke(typeof(IConvertEngine<,>), typeof(ConvertEngine<,>));
            registrationAction.Invoke(typeof(IConvertHelperFactory<,,,>), typeof(ConvertHelperFactory<,,,>));
            registrationAction.Invoke(typeof(ICreateConvertHelper<,,,,>), typeof(CreateConvertHelper<,,,,>));
            registrationAction.Invoke(typeof(ICreateConvertHelper<,,,>), typeof(CreateConvertHelper<,,,>));
            registrationAction.Invoke(typeof(IConvertStrategyProvider<,,>), typeof(ConvertStrategyProvider<,,>));

            // Helper
            registrationAction.Invoke(typeof(IConvertHelper), typeof(ConvertHelper));

            // Value conversion
            registrationAction.Invoke(typeof(IConvertValue<,>), typeof(ValueConverter<,>));

            // Operations
            registrationAction.Invoke(typeof(IOperationCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationCreateFromSourceWithReverseRelation<,,,,>));
            registrationAction.Invoke(typeof(IOperationCreateToManyWithReverseRelation<,,,,,,>), typeof(OperationCreateToManyWithReverseRelation<,,,,,,>));
            registrationAction.Invoke(typeof(IOperationCreateToManyGenericWithReverseRelation<,,,,,,>), typeof(OperationCreateToManyGenericWithReverseRelation<,,,,,,>));
            registrationAction.Invoke(typeof(IOperationCreateToManyGeneric<,,,,,>), typeof(OperationCreateToManyGeneric<,,,,,>));
            registrationAction.Invoke(typeof(IOperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>), typeof(OperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>));
            registrationAction.Invoke(typeof(IOperationMergeLevel<,,,,,,>), typeof(OperationMergeLevel<,,,,,,>));
            registrationAction.Invoke(typeof(IOperationConvertFromSourceOnDifferentLevels<,,,,>), typeof(OperationConvertFromSourceOnDifferentLevels<,,,,>));
            registrationAction.Invoke(typeof(IOperationConvertFromTargetOnDifferentLevels<,,,>), typeof(OperationConvertFromTargetOnDifferentLevels<,,,>));
            registrationAction.Invoke(typeof(IOperationConvertToMany<,,,,>), typeof(OperationConvertToMany<,,,,>));
            registrationAction.Invoke(typeof(IOperationSourceSubConvert<,,,>), typeof(OperationSourceSubConvert<,,,>));
            registrationAction.Invoke(typeof(IOperationTargetSubConvert<,,,>), typeof(OperationTargetSubConvert<,,,>));
            registrationAction.Invoke(typeof(IOperationCopySource<,>), typeof(OperationCopySource<,>));
            registrationAction.Invoke(typeof(IOperationCreateToOneWithReverseRelation<,,,,,>), typeof(OperationCreateToOneWithReverseRelation<,,,,,>));
            registrationAction.Invoke(typeof(IOperationCreateToOne<,,,,,>), typeof(OperationCreateToOne<,,,,,>));
            registrationAction.Invoke(typeof(IOperationCopyFromMany<,,,>), typeof(OperationCopyFromMany<,,,>));
            registrationAction.Invoke(typeof(IOperationCopyValue<,,>), typeof(OperationCopyValue<,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueWithLookUp<,,>), typeof(OperationCopyValueWithLookUp<,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueWithUpperLimit<,,>), typeof(OperationCopyValueWithUpperLimit<,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueIfSourceNotDefault<,,>), typeof(OperationCopyValueIfSourceNotDefault<,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueIfTargetIsDefault<,,>), typeof(OperationCopyValueIfTargetIsDefault<,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueWithSourceFilter<,,>), typeof(OperationCopyValueWithSourceFilter<,,>));
            registrationAction.Invoke(typeof(IOperationSubCopy<,,>), typeof(OperationSubCopy<,,>));
            registrationAction.Invoke(typeof(IOperationSubConvert<,,,,>), typeof(OperationSubConvert<,,,,>));
            registrationAction.Invoke(typeof(IOperationConditionalCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationConditionalCreateFromSourceWithReverseRelation<,,,,>));
            registrationAction.Invoke(typeof(IOperationConditionalCreateToManyWithReverseRelation<,,,,>), typeof(OperationConditionalCreateToManyWithReverseRelation<,,,,>));
            registrationAction.Invoke(typeof(IOperationCopyOneToManyWithReverseRelation<,,,,,,>), typeof(OperationCopyOneToManyWithReverseRelation<,,,,,,>));
            registrationAction.Invoke(typeof(IOperationCopyValueWithMapping<,,,>), typeof(OperationCopyValueWithMapping<,,,>));
            registrationAction.Invoke(typeof(IOperationCopyFromTemporalData<,,,>), typeof(OperationCopyFromTemporalData<,,,>));
        }

        /// <summary>
        /// Registers types needed for copy.
        /// </summary>
        private static void RegisterCopyTypes(Action<Type, Type> registrationAction)
        {
            // Tools
            registrationAction.Invoke(typeof(ICreateCopyHelper<,>), typeof(CreateCopyHelper<,>));
            registrationAction.Invoke(typeof(ICreateCopyHelper<,,>), typeof(CreateCopyHelper<,,>));
            registrationAction.Invoke(typeof(ICopyHelperFactory<,>), typeof(CopyHelperFactory<,>));
            registrationAction.Invoke(typeof(ICopy<>), typeof(Copier<>));
            registrationAction.Invoke(typeof(ICopyHelperRegistrationFactory), typeof(CopyHelperRegistrationFactory));

            // Helper
            registrationAction.Invoke(typeof(ICopyHelper), typeof(CopyHelper));

            // Operations
            registrationAction.Invoke(typeof(ICopyOperationCreateToManyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithReverseRelation<,,>));
            registrationAction.Invoke(typeof(ICopyOperationCreateToOneWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithReverseRelation<,,>));
            registrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategy<,,>), typeof(CopyOperationCreateToManyWithGenericStrategy<,,>));
            registrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>));
            registrationAction.Invoke(typeof(ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>));
            registrationAction.Invoke(typeof(ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>));

            registrationAction.Invoke(typeof(ICopyStrategyProvider<,>), typeof(GenericCopyStrategyProvider<,>));
        }
    }
}
