using BBT.StrategyPattern;
using BBT.StructureTools.Compare;
using BBT.StructureTools.Compare.Helper;
using BBT.StructureTools.Convert;
using BBT.StructureTools.Convert.Strategy;
using BBT.StructureTools.Convert.Value;
using BBT.StructureTools.Copy;
using BBT.StructureTools.Copy.Helper;
using BBT.StructureTools.Copy.Operation;
using BBT.StructureTools.Copy.Strategy;
using BBT.StructureTools.Initialization;
using BBT.StructureTools.Tests.TestTools.IoC;
using Ninject;

namespace BBT.StructureTools.Tests.TestTools
{
    /// <summary>
    /// Utilities to set up and configure for test runs.
    /// </summary>
    /// <remarks>
    /// Keep this code testframework-agnostic!
    /// </remarks>
    public static class Setup
    {
        /// <summary>
        /// Register types for copy, convert, and compare with
        /// Unity and create a resolver which is assigned to the
        /// <see cref="IocHandler.IocResolver"/>. The <see cref="IUnityContainer"/>
        /// which is used within the <see cref="IocHandler.IocResolver"/> is being
        /// returned for further manipulation from within the calling test or test setup method.
        /// </summary>
        public static IKernel SetUpIocResolve()
        {
            var kernel = new StandardKernel();

            // Dependencies from BBT.StrategyPattern
            kernel.Bind(typeof(IStrategyLocator<>)).To(typeof(UnityStrategyLocator<>));
            kernel.Bind(typeof(IInstanceCreator<,>)).To(typeof(GenericInstanceCreator<,>));

            RegisterConvertTypes(kernel);
            RegisterCompareTypes(kernel);
            RegisterCopyTypes(kernel);

            var resolver = new NinjectResolver(kernel);
            IocHandler.Instance.IocResolver = resolver;

            return kernel;
        }

        /// <summary>
        /// Registers types needed for compare within Unity.
        /// </summary>
        public static void RegisterCompareTypes(IKernel container)
        {
            // Tools
            container.Bind(typeof(IComparer<,>)).To(typeof(Comparer<,>));
            container.Bind<IEqualityComparerHelperRegistrationFactory>().To<EqualityComparerHelperRegistrationFactory>();

            // Helper
            container.Bind<ICompareHelper>().To<CompareHelper>();
        }

        /// <summary>
        /// Registers types needed for convert within Unity.
        /// </summary>
        public static void RegisterConvertTypes(IKernel container)
        {
            // Tools
            container.Bind(typeof(IConvert<,,>)).To(typeof(Converter<,,>));
            container.Bind(typeof(IConvertEngine<,>)).To(typeof(ConvertEngine<,>));
            container.Bind(typeof(IConvertHelperFactory<,,,>)).To(typeof(ConvertHelperFactory<,,,>));
            container.Bind(typeof(ICreateConvertHelper<,,,,>)).To(typeof(CreateConvertHelper<,,,,>));
            container.Bind(typeof(ICreateConvertHelper<,,,>)).To(typeof(CreateConvertHelper<,,,>));
            container.Bind(typeof(IConvertStrategyProvider<,,>)).To(typeof(ConvertStrategyProvider<,,>));

            // Helper
            container.Bind<IConvertHelper>().To<ConvertHelper>();

            // Value conversion
            container.Bind(typeof(IConvertValue<,>)).To(typeof(ValueConverter<,>));

            // Operations
            container.Bind(typeof(IOperationCreateFromSourceWithReverseRelation<,,,,>)).To(typeof(OperationCreateFromSourceWithReverseRelation<,,,,>));
            container.Bind(typeof(IOperationCreateToManyWithReverseRelation<,,,,,,>)).To(typeof(OperationCreateToManyWithReverseRelation<,,,,,,>));
            container.Bind(typeof(IOperationCreateToManyGenericWithReverseRelation<,,,,,,>)).To(typeof(OperationCreateToManyGenericWithReverseRelation<,,,,,,>));
            container.Bind(typeof(IOperationCreateToManyGeneric<,,,,,>)).To(typeof(OperationCreateToManyGeneric<,,,,,>));
            container.Bind(typeof(IOperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>)).To(typeof(OperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>));
            container.Bind(typeof(IOperationMergeLevel<,,,,,,>)).To(typeof(OperationMergeLevel<,,,,,,>));
            container.Bind(typeof(IOperationConvertFromSourceOnDifferentLevels<,,,,>)).To(typeof(OperationConvertFromSourceOnDifferentLevels<,,,,>));
            container.Bind(typeof(IOperationConvertFromTargetOnDifferentLevels<,,,>)).To(typeof(OperationConvertFromTargetOnDifferentLevels<,,,>));
            container.Bind(typeof(IOperationConvertToMany<,,,,>)).To(typeof(OperationConvertToMany<,,,,>));
            container.Bind(typeof(IOperationSourceSubConvert<,,,>)).To(typeof(OperationSourceSubConvert<,,,>));
            container.Bind(typeof(IOperationTargetSubConvert<,,,>)).To(typeof(OperationTargetSubConvert<,,,>));
            container.Bind(typeof(IOperationCopySource<,>)).To(typeof(OperationCopySource<,>));
            container.Bind(typeof(IOperationCreateToOneWithReverseRelation<,,,,,>)).To(typeof(OperationCreateToOneWithReverseRelation<,,,,,>));
            container.Bind(typeof(IOperationCreateToOne<,,,,,>)).To(typeof(OperationCreateToOne<,,,,,>));
            container.Bind(typeof(IOperationCopyFromMany<,,,>)).To(typeof(OperationCopyFromMany<,,,>));
            container.Bind(typeof(IOperationCopyValue<,,>)).To(typeof(OperationCopyValue<,,>));
            container.Bind(typeof(IOperationCopyValueWithLookUp<,,>)).To(typeof(OperationCopyValueWithLookUp<,,>));
            container.Bind(typeof(IOperationCopyValueWithUpperLimit<,,>)).To(typeof(OperationCopyValueWithUpperLimit<,,>));
            container.Bind(typeof(IOperationCopyValueIfSourceNotDefault<,,>)).To(typeof(OperationCopyValueIfSourceNotDefault<,,>));
            container.Bind(typeof(IOperationCopyValueIfTargetIsDefault<,,>)).To(typeof(OperationCopyValueIfTargetIsDefault<,,>));
            container.Bind(typeof(IOperationCopyValueWithSourceFilter<,,>)).To(typeof(OperationCopyValueWithSourceFilter<,,>));
            container.Bind(typeof(IOperationSubCopy<,,>)).To(typeof(OperationSubCopy<,,>));
            container.Bind(typeof(IOperationSubConvert<,,,,>)).To(typeof(OperationSubConvert<,,,,>));
            container.Bind(typeof(IOperationConditionalCreateFromSourceWithReverseRelation<,,,,>)).To(typeof(OperationConditionalCreateFromSourceWithReverseRelation<,,,,>));
            container.Bind(typeof(IOperationConditionalCreateToManyWithReverseRelation<,,,,>)).To(typeof(OperationConditionalCreateToManyWithReverseRelation<,,,,>));
            container.Bind(typeof(IOperationCopyOneToManyWithReverseRelation<,,,,,,>)).To(typeof(OperationCopyOneToManyWithReverseRelation<,,,,,,>));
            container.Bind(typeof(IOperationCopyValueWithMapping<,,,>)).To(typeof(OperationCopyValueWithMapping<,,,>));
        }

        /// <summary>
        /// Registers types needed for copy within Unity.
        /// </summary>
        public static void RegisterCopyTypes(IKernel container)
        {
            // Tools
            container.Bind(typeof(ICreateCopyHelper<,>)).To(typeof(CreateCopyHelper<,>));
            container.Bind(typeof(ICreateCopyHelper<,,>)).To(typeof(CreateCopyHelper<,,>));
            container.Bind(typeof(ICopyHelperFactory<,>)).To(typeof(CopyHelperFactory<,>));
            container.Bind(typeof(ICopy<>)).To(typeof(Copier<>));
            container.Bind<ICopyHelperRegistrationFactory>().To<CopyHelperRegistrationFactory>();

            // Helper
            container.Bind<ICopyHelper>().To<CopyHelper>();

            // Operations
            container.Bind(typeof(ICopyOperationCreateToManyWithReverseRelation<,,>)).To(typeof(CopyOperationCreateToManyWithReverseRelation<,,>));
            container.Bind(typeof(ICopyOperationCreateToOneWithReverseRelation<,,>)).To(typeof(CopyOperationCreateToOneWithReverseRelation<,,>));
            container.Bind(typeof(ICopyOperationCreateToManyWithGenericStrategy<,,>)).To(typeof(CopyOperationCreateToManyWithGenericStrategy<,,>));
            container.Bind(typeof(ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>)).To(typeof(CopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>));
            container.Bind(typeof(ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>)).To(typeof(CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>));
            container.Bind(typeof(ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>)).To(typeof(CopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>));


            container.Bind(typeof(ICopyStrategyProvider<,>)).To(typeof(GenericCopyStrategyProvider<,>));
        }
    }
}
