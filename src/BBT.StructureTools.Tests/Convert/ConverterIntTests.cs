namespace BBT.StructureTools.Tests.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Convert.Value;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Provider;
    using BBT.StructureTools.Strategy;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.Registrations.WholeStructure;
    using BBT.StructureTools.Tests.Convert.TestData;
    using BBT.StructureTools.Tests.TestTools;
    using Ninject;
    using Shouldly;
    using Xunit;

    /// <summary>
    /// Test class convert registrations.
    /// </summary>
    public class ConverterIntTests
    {
        #region Setup
        private static IKernel container;

        public ConverterIntTests()
        {
            container = TestIocContainer.Initialize();
        }

        #endregion

        #region Convert

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_WholeStructure_ConvertAll()
        {
            // Arrange
            DoConvertRegistrationsForWholeStructure();
            var converter = GetConverter<SourceRoot, TargetRoot>();

            var sourceRoot = CreateSourceRoot();
            var targetRoot = new TargetRoot()
            {
                NumberSourceDefault = 100,
                NumberSourceNotDefault = 200,
                NumberTargetDefault = 0,
                NumberTargetNotDefault = 123.45m,
                ReferenceDate = new DateTime(1993, 10, 5),
            };

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            processings.Add(new GenericContinueConvertInterception<MasterData, TargetRoot>(x => x.IsDefault));
            converter.Convert(sourceRoot, targetRoot, processings);

            // Assert
            AssertRoot(sourceRoot, targetRoot);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CopyAttribute_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CopyTreeAttributeRegistrations>();

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree()
            {
                TreeName = "TreeName",
            };

            var targetTree = new TargetTree()
            {
                MasterDataId = Guid.NewGuid(),
            };

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            targetTree.TreeName.ShouldBe(sourceTree.TreeName);
            targetTree.TemporalDataOriginId.ShouldBe(targetTree.MasterDataId);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToManyWithReverseRelation_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyWithReverseRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            var targetTree = new TargetTree();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            sourceTree.Leafs.Count.ShouldBe(targetTree.TargetLeafs.Count);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToManyWithGenericReverseRelation_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyGenericWithReverseRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            var targetTree = new TargetTree();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            sourceTree.Leafs.Count.ShouldBe(targetTree.TargetLeafs.Count);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToMany_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyGenericRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            var targetTree = new TargetTree();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            sourceTree.Leafs.Count.ShouldBe(targetTree.TargetLeafs.Count);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_Processings_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyGenericRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            var targetTree = new TargetTree();
            var targetIds = new List<Guid>();
            var sourceIds = new List<Guid>();
            var preProcHits = 0;

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            processings.Add(new GenericConvertPostProcessing<SourceTreeLeaf, TargetTreeLeaf>((x, y) => targetIds.Add(y.Id)));
            processings.Add(new GenericContinueConvertInterception<SourceTreeLeaf, TargetTreeLeaf>(
                x =>
                {
                    sourceIds.Add(x.Id);
                    return true;
                }));
            processings.Add(new GenericConvertPreProcessing<SourceTreeLeaf, TargetTreeLeaf>((x, y) => preProcHits++));

            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            sourceTree.Leafs.Count.ShouldBe(targetTree.TargetLeafs.Count);
            targetIds.ShouldBe(targetTree.TargetLeafs.Select(x => x.Id).ToList(), ignoreOrder: true);
            sourceIds.ShouldBe(sourceTree.Leafs.Select(x => x.Id).ToList(), ignoreOrder: true);
            preProcHits.ShouldBe(2);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToOneHistWithCondition_Successful()
        {
            // Arrange
            container.Bind(typeof(ITemporalDataHandler<>)).To(typeof(TemporalDataHandler<>));
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToOneHistWithConditionRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeHist, TargetTreeHist, IForTest>>().To<CopyHistAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            var expFilteredHist = CreateTreeHist(new DateTime(2000, 1, 1), new DateTime(2020, 12, 31));
            var expEndDate = new DateTime(9999, 12, 31);
            sourceTree.Hists.Add(expFilteredHist);
            sourceTree.Hists.Add(CreateTreeHist(new DateTime(2021, 1, 1), new DateTime(2040, 12, 31)));
            var targetRoot = new TargetRoot()
            {
                ReferenceDate = new DateTime(2015, 6, 12),
            };

            var targetTree = new TargetTree() { TargetRoot = targetRoot };
            targetRoot.TargetTree = targetTree;

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            targetTree.TargetHists.Count.ShouldBe(1);
            var targetHist = targetTree.TargetHists.Single();
            targetHist.OriginId.ShouldBe(expFilteredHist.Id);
            targetHist.From.ShouldBe(expFilteredHist.From);
            targetHist.To.ShouldBe(expEndDate);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToManyFromGenericStrategyWithReverseRelation_Successful()
        {
            // Arrange
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));
            container.Bind<IConvertRegistrations<SourceRoot, TargetRoot, IForTest>>().To<CreateToManyFromGenericStrategyWithReverseRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>>().To<DerivedLeafToTargetDerivedLeafConvertRegistrations>();
            container.Bind<ICreateByBaseAsCriterionStrategy<SourceBaseLeaf, TargetBaseLeaf>>().To<GenericCreateByBaseAsCriterionStrategy<SourceBaseLeaf, SourceDerivedLeaf, TargetBaseLeaf, TargetDerivedLeaf, TargetDerivedLeaf>>();
            container.Bind<ISourceConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest>>().To<GenericSourceConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest, SourceDerivedLeaf, TargetDerivedLeaf>>();

            var converter = GetConverter<SourceRoot, TargetRoot>();

            var sourceRoot = new SourceRoot();
            sourceRoot.Leafs.Add(new SourceDerivedLeaf() { Root = sourceRoot });
            sourceRoot.Leafs.Add(new SourceDerivedLeaf() { Root = sourceRoot });

            var targetRoot = new TargetRoot();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceRoot, targetRoot, processings);

            // Assert
            targetRoot.TargetLeafs.Count.ShouldBe(sourceRoot.Leafs.Count);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_CreateToManyFromStrategyHelperWithReverseRelation_Successful()
        {
            // Arrange
            container.Bind(typeof(ICreateConvertFromStrategyHelperFactory<,,>)).To(typeof(CreateConvertFromStrategyHelperFactory<,,>));
            container.Bind(typeof(ICreateConvertFromStrategyHelper<,,,>)).To(typeof(CreateConvertFromStrategyHelperReverse<,,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));
            container.Bind<IConvertRegistrations<SourceRoot, TargetRoot, IForTest>>().To<CreateToManyFromStrategyHelperWithReverseRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>>().To<DerivedLeafToTargetDerivedLeafConvertRegistrations>();
            container.Bind<ICreateConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest>>().To<CreateConvertStrategy<SourceBaseLeaf, SourceDerivedLeaf, TargetBaseLeaf, TargetDerivedLeaf, TargetDerivedLeaf, IForTest>>();

            var converter = GetConverter<SourceRoot, TargetRoot>();

            var sourceRoot = new SourceRoot();
            sourceRoot.Leafs.Add(new SourceDerivedLeaf() { Root = sourceRoot });
            sourceRoot.Leafs.Add(new SourceDerivedLeaf() { Root = sourceRoot });

            var targetRoot = new TargetRoot();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceRoot, targetRoot, processings);

            // Assert
            targetRoot.TargetLeafs.Count.ShouldBe(sourceRoot.Leafs.Count);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_RegisterCreateToOneWithRelation_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceRoot, TargetRoot, IForTest>>().To<CreateToOneWithRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CopyTreeAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceRoot, TargetRoot>();

            var sourceRoot = new SourceRoot()
            {
                Tree = new SourceTree(),
            };
            var targetRoot = new TargetRoot();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceRoot, targetRoot, processings);

            // Assert
            targetRoot.TargetTree.ShouldNotBeNull();
            targetRoot.TargetTree.RelationOnTarget.ShouldBe(targetRoot.RelationOnTarget);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_RegisterCreateToOneWithRelationAndTarget_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceRoot, TargetRoot, IForTest>>().To<CreateToOneWithRelationAndTargetRegistrations>();
            container.Bind<IConvertRegistrations<IdDto, TargetTree, IForTest>>().To<CopyIdDtoToTargetTreeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceRoot, TargetRoot>();

            var sourceRoot = new SourceRoot()
            {
                Tree = new SourceTree(),
            };
            var targetRoot = new TargetRoot()
            {
                Id = Guid.NewGuid(),
            };

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceRoot, targetRoot, processings);

            // Assert
            targetRoot.TargetTree.ShouldNotBeNull();
            targetRoot.TargetTree.RelationOnTarget.ShouldBe(targetRoot.RelationOnTarget);
            targetRoot.TargetTree.TemporalDataOriginId.ShouldBe(targetRoot.Id);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_RegisterCreateToManyWithRelation_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyWithRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree()
            {
            };

            sourceTree.Leafs.Add(new SourceTreeLeaf());

            var targetTree = new TargetTree()
            {
                RelationOnTarget = new MasterData(),
            };

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            targetTree.TargetLeafs.Count.ShouldBe(1);
            var targetLeaf = targetTree.TargetLeafs.Single();
            targetLeaf.RelationOnTarget.ShouldBe(targetTree.RelationOnTarget);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_RegisterCreateToManyWithRelationAndTarget_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyWithRelationAndTargetRegistrations>();
            container.Bind<IConvertRegistrations<IdDto, TargetTreeLeaf, IForTest>>().To<CopyIdDtoToTargetTreeLeafRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree()
            {
            };

            sourceTree.Leafs.Add(new SourceTreeLeaf());

            var targetTree = new TargetTree()
            {
                RelationOnTarget = new MasterData(),
            };

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            targetTree.TargetLeafs.Count.ShouldBe(1);
            var targetLeaf = targetTree.TargetLeafs.Single();
            targetLeaf.RelationOnTarget.ShouldBe(targetTree.RelationOnTarget);
            targetLeaf.OriginId.ShouldBe(targetTree.Id);
        }

        /// <summary>
        /// Tests the convert from root to leaf into target structure.
        /// </summary>
        [Fact]
        public void Convert_RegisterCreateToManyWithRelation_ReverseRelation_Successful()
        {
            // Arrange
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<CreateToManyWithRelationReverseRelationRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var converter = GetConverter<SourceTree, TargetTree>();

            var sourceTree = new SourceTree();
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            sourceTree.Leafs.Add(new SourceTreeLeaf() { Tree = sourceTree });
            var targetTree = new TargetTree();

            // Act
            var processings = new List<IBaseAdditionalProcessing>();
            converter.Convert(sourceTree, targetTree, processings);

            // Assert
            sourceTree.Leafs.Count.ShouldBe(targetTree.TargetLeafs.Count);
            for (var i = 0; i < sourceTree.Leafs.Count; i++)
            {
                targetTree.TargetLeafs[i].TargetTree.ShouldBe(targetTree);
                targetTree.TargetLeafs[i].OriginId.ShouldBe(sourceTree.Leafs.ElementAt(i).Id);
            }
        }

        #endregion

        #region Asserts

        private static void AssertRoot(SourceRoot sourceRoot, TargetRoot targetRoot)
        {
            targetRoot.OriginRoot.ShouldBe(sourceRoot);
            targetRoot.Name.ShouldBe(sourceRoot.Name);
            targetRoot.RootId.ShouldBe(sourceRoot.Id);
            targetRoot.Id.ShouldBe(sourceRoot.Id);
            targetRoot.TargetId.ShouldBe(sourceRoot.Id);
            targetRoot.NumberSourceDefault.ShouldNotBe(sourceRoot.NumberSourceDefault);
            targetRoot.NumberSourceNotDefault.ShouldBe(sourceRoot.NumberSourceNotDefault);
            targetRoot.NumberTargetDefault.ShouldBe(sourceRoot.NumberTargetDefault);
            targetRoot.NumberTargetNotDefault.ShouldNotBe(sourceRoot.NumberTargetNotDefault);
            targetRoot.NumberSourceLookedUp.ShouldBe(sourceRoot.NumberSourceNotDefault);
            targetRoot.NumberSourceNotLookedUp.ShouldBe(sourceRoot.NumberSourceNotDefault);

            // ToDo: Check is this a bug? see https://github.com/bbtsoftware/BBT.StructureTools/issues/65.
            // targetRoot.NumberLimitNotApplied.ShouldBe(sourceRoot.NumberSourceDefault);
            targetRoot.NumberLimitApplied.ShouldNotBe(sourceRoot.NumberSourceNotDefault);
            targetRoot.EnumValue.ShouldBe(sourceRoot.EnumValue.Target);
            targetRoot.FilteredHist.ShouldBe(sourceRoot.Tree.ExpectedFilteredHist);
            ((TargetDerivedLeaf)targetRoot.TargetLeaf).OriginId.ShouldBe(((SourceDerivedLeaf)sourceRoot.Leaf).Id);

            targetRoot.TargetMasterData.OriginId.ShouldBe(sourceRoot.ExpectedFilteredMasterData.Id);
            targetRoot.TargetMasterData.IsDefault.ShouldBe(sourceRoot.ExpectedFilteredMasterData.IsDefault);
            targetRoot.FilteredMasterDataId.ShouldBe(sourceRoot.ExpectedFilteredMasterData.Id);

            AssertTree(sourceRoot, targetRoot);
        }

        private static void AssertTree(SourceRoot sourceRoot, TargetRoot targetRoot)
        {
            var sourceTree = sourceRoot.Tree;
            var targetTree = targetRoot.TargetTree;

            targetRoot.TargetTreeId.ShouldBe(targetTree.Id);
            targetRoot.OriginTreeId.ShouldBe(sourceTree.Id);
            targetRoot.TreeId.ShouldBe(sourceTree.Id);
            targetTree.TargetRoot.ShouldBe(targetRoot);
            targetTree.OriginTree.ShouldBe(sourceRoot.Tree);
            targetTree.TreeName.ShouldBe(sourceTree.TreeName);
            targetTree.MasterDataId.ShouldBe(sourceTree.MasterData.Id);
            targetTree.TemporalDataOriginId.ShouldBe(sourceTree.ExpectedFilteredHist.Id);
            targetTree.BaseDataId.ShouldBe(sourceTree.BaseDataId);

            AssertTreeLeafs(sourceTree, targetTree);
            AssertTreeHistLeafs(sourceTree, targetTree);
        }

        private static void AssertTreeLeafs(SourceTree sourceTree, TargetTree targetTree)
        {
            targetTree.TargetLeafs.Count.ShouldBe(sourceTree.Leafs.Count);

            for (var i = 0; i < sourceTree.Leafs.Count; i++)
            {
                var targetLeaf = targetTree.TargetLeafs.ElementAt(i);
                targetLeaf.TargetTree.ShouldBe(targetTree);
                AssertTreeLeaf(sourceTree.Leafs.ElementAt(i), targetLeaf);
            }
        }

        private static void AssertTreeLeaf(SourceTreeLeaf sourceLeaf, TargetTreeLeaf targetLeaf)
        {
            targetLeaf.OriginId.ShouldBe(sourceLeaf.Id);
            targetLeaf.MasterDataId.ShouldBe(sourceLeaf.MasterDatas.First().Id);
            targetLeaf.LeafName.ShouldBe(sourceLeaf.LeafName);
            targetLeaf.Child.OriginId.ShouldBe(sourceLeaf.Id);
        }

        private static void AssertTreeHistLeafs(SourceTree sourceTree, TargetTree targetTree)
        {
            var sourceTreeHistLeafs = sourceTree.Leafs.SelectMany(x => x.TreeHistLeafs).ToList();
            targetTree.TargetHistLeafs.Count.ShouldBe(sourceTreeHistLeafs.Count);

            for (var i = 0; i < sourceTreeHistLeafs.Count; i++)
            {
                var targetHistLeaf = targetTree.TargetHistLeafs.ElementAt(i);
                targetHistLeaf.TargetTree.ShouldBe(targetTree);
                AssertTreeHistLeaf(sourceTreeHistLeafs.ElementAt(i), targetHistLeaf);
            }
        }

        private static void AssertTreeHistLeaf(SourceTreeHistLeaf sourceHistLeaf, TargetTreeHistLeaf targetHistLeaf)
        {
            targetHistLeaf.OriginTreeHistLeafId.ShouldBe(sourceHistLeaf.TreeHistLeafId);
        }

        #endregion

        #region Create test data

        private static SourceRoot CreateSourceRoot()
        {
            var root = new SourceRoot()
            {
                Id = Guid.NewGuid(),
                NumberSourceDefault = 0,
                NumberSourceNotDefault = 15,
                NumberTargetDefault = 1111.2m,
                NumberTargetNotDefault = 7645.3m,
                EnumValue = (SourceEnum.Value2, TargetEnum.Value2),
            };
            root.CreateStringForProperty(x => x.Name);

            var tree = CreateSourceTree();

            var leaf = new SourceDerivedLeaf() { Root = root };
            root.Leaf = leaf;

            var expecedFilteredMasterData = CreateMasterData(true);

            root.ExpectedFilteredMasterData = expecedFilteredMasterData;
            root.MasterDatasToFilter.Add(CreateMasterData());
            root.MasterDatasToFilter.Add(expecedFilteredMasterData);

            root.Tree = tree;
            tree.Root = root;
            return root;
        }

        private static SourceTree CreateSourceTree()
        {
            var tree = new SourceTree();
            tree.CreateStringForProperty(x => x.TreeName);

            tree.MasterData = CreateMasterData();

            var leaf = CreateSourceLeaf();
            tree.Leafs.Add(leaf);
            var leaf2 = CreateSourceLeaf();
            tree.Leafs.Add(leaf2);
            leaf.Tree = tree;
            leaf2.Tree = tree;

            var hist = CreateTreeHist(new DateTime(1993, 10, 4), new DateTime(2019, 10, 29));
            tree.Hists.Add(hist);
            var hist2 = CreateTreeHist(new DateTime(1994, 10, 7), new DateTime(2019, 10, 3));
            tree.Hists.Add(hist2);

            tree.ExpectedFilteredHist = hist;
            CreateTreeHistLeaf(hist, leaf);
            CreateTreeHistLeaf(hist2, leaf2);

            return tree;
        }

        private static MasterData CreateMasterData(bool isDefault = false)
        {
            var treeMasterData = new MasterData()
            {
                IsDefault = isDefault,
            };
            return treeMasterData;
        }

        private static SourceTreeLeaf CreateSourceLeaf()
        {
            var leaf = new SourceTreeLeaf()
            {
                TemporalDataRefDate = new DateTime(1993, 10, 5),
            };

            leaf.CreateStringForProperty(x => x.LeafName);

            var leafMasterData = CreateSourceLeafMasterData(true);
            leaf.MasterDatas.Add(leafMasterData);
            var leafMasterData2 = CreateSourceLeafMasterData();
            leaf.MasterDatas.Add(leafMasterData2);

            return leaf;
        }

        private static SourceTreeHist CreateTreeHist(
            DateTime begin,
            DateTime end)
        {
            return new SourceTreeHist
            {
                From = begin,
                To = end,
            };
        }

        private static SourceTreeHistLeaf CreateTreeHistLeaf(
            SourceTreeHist treeHist,
            SourceTreeLeaf leaf)
        {
            var treeHistLeaf = new SourceTreeHistLeaf()
            {
                TreeHist = treeHist,
                Leaf = leaf,
                TreeHistLeafId = Guid.NewGuid(),
            };

            treeHist.TreeHistLeafs.Add(treeHistLeaf);
            leaf.TreeHistLeafs.Add(treeHistLeaf);
            return treeHistLeaf;
        }

        private static MasterData CreateSourceLeafMasterData(
            bool isDefault = false)
        {
            var sourceLeafMasterData = new MasterData
            {
                IsDefault = isDefault,
            };

            return sourceLeafMasterData;
        }

        #endregion

        #region Do registrations

        private static IConvert<TSource, TTarget, IForTest> GetConverter<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            return container.Get<IConvert<TSource, TTarget, IForTest>>();
        }

        private static void DoConvertRegistrationsForWholeStructure()
        {
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));
            container.Bind(typeof(ITemporalDataHandler<>)).To(typeof(TemporalDataHandler<>));
            container.Bind<IConvertRegistrations<SourceRoot, TargetRoot, IForTest>>().To<RootToTargetRootConvertRegistrations>();
            container.Bind<IConvertRegistrations<RootBase, TargetRoot, IForTest>>().To<RootBaseToTargetRootConvertRegistration>();
            container.Bind<IConvertRegistrations<RootBase, RootBase, IForTest>>().To<RootBaseToRootBaseConvertRegistration>();
            container.Bind<IConvertRegistrations<SourceRoot, RootBase, IForTest>>().To<RootToRootBaseConvertRegistration>();
            container.Bind<IConvertRegistrations<SourceTree, TargetTree, IForTest>>().To<TreeToTargetTreeConvertRegistrations>();
            container.Bind<IConvertRegistrations<MasterData, TargetTree, IForTest>>().To<MasterDataToTargetTreeConvertRegistrations>();
            container.Bind<IConvertRegistrations<TargetTree, TargetRoot, IForTest>>().To<TargetTreeToTargetRootConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTree, TargetRoot, IForTest>>().To<TreeToTargetRootConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTree, RootBase, IForTest>>().To<TreeToRootBaseConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeafChild, IForTest>>().To<TreeLeafToTargetTreeLeafChildConvertRegistrations>();
            container.Bind<IConvertRegistrations<MasterData, TargetMasterData, IForTest>>().To<MasterDataToTargetMasterDataConvertRegistrations>();
            container.Bind<IConvertRegistrations<MasterData, TargetRoot, IForTest>>().To<MasterDataToTargetRootConvertRegistrations>();

            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<TreeLeafToTargetTreeLeafConvertRegistrations>();
            container.Bind<IConvertRegistrations<MasterData, TargetTreeLeaf, IForTest>>().To<MasterDataToTargetLeafConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeHist, TargetTree, IForTest>>().To<TreeHistToTargetTreeConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeHistLeaf, TargetTreeLeaf, IForTest>>().To<TreeHistLeafToTargetTreeLeafConvertRegistrations>();
            container.Bind<IConvertRegistrations<SourceTreeHistLeaf, TargetTreeHistLeaf, IForTest>>().To<TreeHistLeafToTargetTreeHistLeafConvertRegistrations>();

            container.Bind<ICopyRegistrations<ITemporalData>>().To<TemporalDataCopyRegistrations>();
            container.Bind<IConvertValueRegistrations<SourceEnum, TargetEnum>>().To<EnumConvertValueRegistrations>();

            container.Bind<IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>>().To<DerivedLeafToTargetDerivedLeafConvertRegistrations>();
            container.Bind<ICreateByBaseAsCriterionStrategy<SourceBaseLeaf, TargetBaseLeaf>>().To<GenericCreateByBaseAsCriterionStrategy<SourceBaseLeaf, SourceDerivedLeaf, TargetBaseLeaf, TargetDerivedLeaf, TargetDerivedLeaf>>();
            container.Bind<ISourceConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest>>().To<GenericSourceConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest, SourceDerivedLeaf, TargetDerivedLeaf>>();
            container.Bind<ICopyRegistrations<BaseData>>().To<BaseDataCopyRegistrations>();
        }

        #endregion
    }
}
