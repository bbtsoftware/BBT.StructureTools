using System;
using System.Collections.Generic;
using System.Text;
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
using Unity;

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
        public static IUnityContainer SetUpIocResolve()
        {
            var container = new UnityContainer();

            RegisterConvertTypes(container);
            RegisterCompareTypes(container);
            RegisterCopyTypes(container);

            var resolver = new UnityResolver(container);
            IocHandler.Instance.IocResolver = resolver;

            return container;
        }

        /// <summary>
        /// Registers types needed for compare within Unity.
        /// </summary>
        public static void RegisterCompareTypes(IUnityContainer container)
        {
            // Tools
            container.RegisterType(typeof(IComparer<,>), typeof(Comparer<,>));
            container.RegisterType<IEqualityComparerHelperRegistrationFactory, EqualityComparerHelperRegistrationFactory>();

            // Helper
            container.RegisterType<ICompareHelper, CompareHelper>();
        }

        /// <summary>
        /// Registers types needed for convert within Unity.
        /// </summary>
        public static void RegisterConvertTypes(IUnityContainer container)
        {
            // Tools
            container.RegisterType(typeof(IConvert<,,>), typeof(Converter<,,>));
            container.RegisterType(typeof(IConvertEngine<,>), typeof(ConvertEngine<,>));
            container.RegisterType(typeof(IConvertHelperFactory<,,,>), typeof(ConvertHelperFactory<,,,>));
            container.RegisterType(typeof(ICreateConvertHelper<,,,,>), typeof(CreateConvertHelper<,,,,>));
            container.RegisterType(typeof(ICreateConvertHelper<,,,>), typeof(CreateConvertHelper<,,,>));
            container.RegisterType(typeof(IConvertStrategyProvider<,,>), typeof(ConvertStrategyProvider<,,>));

            // Helper
            container.RegisterType<IConvertHelper, ConvertHelper>();

            // Value conversion
            container.RegisterType(typeof(IConvertValue<,>), typeof(ValueConverter<,>));

            // Operations
            container.RegisterType(typeof(IOperationCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationCreateFromSourceWithReverseRelation<,,,,>));
            container.RegisterType(typeof(IOperationCreateToManyWithReverseRelation<,,,,,,>), typeof(OperationCreateToManyWithReverseRelation<,,,,,,>));
            container.RegisterType(typeof(IOperationCreateToManyGenericWithReverseRelation<,,,,,,>), typeof(OperationCreateToManyGenericWithReverseRelation<,,,,,,>));
            container.RegisterType(typeof(IOperationCreateToManyGeneric<,,,,,>), typeof(OperationCreateToManyGeneric<,,,,,>));
            container.RegisterType(typeof(IOperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>), typeof(OperationCreateToManyWithSourceFilterAndReverseRelation<,,,,,,>));
            container.RegisterType(typeof(IOperationMergeLevel<,,,,,,>), typeof(OperationMergeLevel<,,,,,,>));
            container.RegisterType(typeof(IOperationConvertFromSourceOnDifferentLevels<,,,,>), typeof(OperationConvertFromSourceOnDifferentLevels<,,,,>));
            container.RegisterType(typeof(IOperationConvertFromTargetOnDifferentLevels<,,,>), typeof(OperationConvertFromTargetOnDifferentLevels<,,,>));
            container.RegisterType(typeof(IOperationConvertToMany<,,,,>), typeof(OperationConvertToMany<,,,,>));
            container.RegisterType(typeof(IOperationSourceSubConvert<,,,>), typeof(OperationSourceSubConvert<,,,>));
            container.RegisterType(typeof(IOperationTargetSubConvert<,,,>), typeof(OperationTargetSubConvert<,,,>));
            container.RegisterType(typeof(IOperationCopySource<,>), typeof(OperationCopySource<,>));
            container.RegisterType(typeof(IOperationCreateToOneWithReverseRelation<,,,,,>), typeof(OperationCreateToOneWithReverseRelation<,,,,,>));
            container.RegisterType(typeof(IOperationCreateToOne<,,,,,>), typeof(OperationCreateToOne<,,,,,>));
            container.RegisterType(typeof(IOperationCopyFromMany<,,,>), typeof(OperationCopyFromMany<,,,>));
            container.RegisterType(typeof(IOperationCopyValue<,,>), typeof(OperationCopyValue<,,>));
            container.RegisterType(typeof(IOperationCopyValueWithLookUp<,,>), typeof(OperationCopyValueWithLookUp<,,>));
            container.RegisterType(typeof(IOperationCopyValueWithUpperLimit<,,>), typeof(OperationCopyValueWithUpperLimit<,,>));
            container.RegisterType(typeof(IOperationCopyValueIfSourceNotDefault<,,>), typeof(OperationCopyValueIfSourceNotDefault<,,>));
            container.RegisterType(typeof(IOperationCopyValueIfTargetIsDefault<,,>), typeof(OperationCopyValueIfTargetIsDefault<,,>));
            container.RegisterType(typeof(IOperationCopyValueWithSourceFilter<,,>), typeof(OperationCopyValueWithSourceFilter<,,>));
            container.RegisterType(typeof(IOperationSubCopy<,,>), typeof(OperationSubCopy<,,>));
            container.RegisterType(typeof(IOperationSubConvert<,,,,>), typeof(OperationSubConvert<,,,,>));
            container.RegisterType(typeof(IOperationConditionalCreateFromSourceWithReverseRelation<,,,,>), typeof(OperationConditionalCreateFromSourceWithReverseRelation<,,,,>));
            container.RegisterType(typeof(IOperationConditionalCreateToManyWithReverseRelation<,,,,>), typeof(OperationConditionalCreateToManyWithReverseRelation<,,,,>));
            container.RegisterType(typeof(IOperationCopyOneToManyWithReverseRelation<,,,,,,>), typeof(OperationCopyOneToManyWithReverseRelation<,,,,,,>));
            container.RegisterType(typeof(IOperationCopyValueWithMapping<,,,>), typeof(OperationCopyValueWithMapping<,,,>));
        }

        /// <summary>
        /// Registers types needed for copy within Unity.
        /// </summary>
        public static void RegisterCopyTypes(IUnityContainer container)
        {
            // Tools
            container.RegisterType(typeof(ICreateCopyHelper<,>), typeof(CreateCopyHelper<,>));
            container.RegisterType(typeof(ICreateCopyHelper<,,>), typeof(CreateCopyHelper<,,>));
            container.RegisterType(typeof(ICopyHelperFactory<,>), typeof(CopyHelperFactory<,>));
            container.RegisterType(typeof(ICopy<>), typeof(Copier<>));
            container.RegisterType<ICopyHelperRegistrationFactory, CopyHelperRegistrationFactory>();

            // Helper
            container.RegisterType<ICopyHelper, CopyHelper>();

            // Operations
            container.RegisterType(typeof(ICopyOperationCreateToManyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithReverseRelation<,,>));
            container.RegisterType(typeof(ICopyOperationCreateToOneWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithReverseRelation<,,>));
            container.RegisterType(typeof(ICopyOperationCreateToManyWithGenericStrategy<,,>), typeof(CopyOperationCreateToManyWithGenericStrategy<,,>));
            container.RegisterType(typeof(ICopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyWithReverseRelation<,,>));
            container.RegisterType(typeof(ICopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>), typeof(CopyOperationCreateToManyWithGenericStrategyReverseRelationOnly<,,>));
            container.RegisterType(typeof(ICopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>), typeof(CopyOperationCreateToOneWithGenericStrategyWithReverseRelation<,,>));


            container.RegisterType(typeof(ICopyStrategyProvider<,>), typeof(GenericCopyStrategyProvider<,>));
        }
    }
}
