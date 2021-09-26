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
    using FluentAssertions;
    using Ninject;
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
            targetTree.TreeName.Should().Be(sourceTree.TreeName);
            targetTree.TemporalDataOriginId.Should().Be(targetTree.MasterDataId);
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
            sourceTree.Leafs.Count.Should().Be(targetTree.TargetLeafs.Count);
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
            sourceTree.Leafs.Count.Should().Be(targetTree.TargetLeafs.Count);
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
            sourceTree.Leafs.Count.Should().Be(targetTree.TargetLeafs.Count);
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
            sourceTree.Leafs.Count.Should().Be(targetTree.TargetLeafs.Count);
            targetIds.Should().BeEquivalentTo(targetTree.TargetLeafs.Select(x => x.Id).ToList());
            sourceIds.Should().BeEquivalentTo(sourceTree.Leafs.Select(x => x.Id).ToList());
            preProcHits.Should().Be(2);
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
            targetTree.TargetHists.Count.Should().Be(1);
            var targetHist = targetTree.TargetHists.Single();
            targetHist.OriginId.Should().Be(expFilteredHist.Id);
            targetHist.From.Should().Be(expFilteredHist.From);
            targetHist.To.Should().Be(expEndDate);
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
            targetRoot.TargetLeafs.Count.Should().Be(sourceRoot.Leafs.Count);
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
            targetRoot.TargetTree.Should().NotBeNull();
            targetRoot.TargetTree.RelationOnTarget.Should().Be(targetRoot.RelationOnTarget);
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
            targetRoot.TargetTree.Should().NotBeNull();
            targetRoot.TargetTree.RelationOnTarget.Should().Be(targetRoot.RelationOnTarget);
            targetRoot.TargetTree.TemporalDataOriginId.Should().Be(targetRoot.Id);
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
            targetTree.TargetLeafs.Count.Should().Be(1);
            var targetLeaf = targetTree.TargetLeafs.Single();
            targetLeaf.RelationOnTarget.Should().Be(targetTree.RelationOnTarget);
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
            targetTree.TargetLeafs.Count.Should().Be(1);
            var targetLeaf = targetTree.TargetLeafs.Single();
            targetLeaf.RelationOnTarget.Should().Be(targetTree.RelationOnTarget);
            targetLeaf.OriginId.Should().Be(targetTree.Id);
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
            sourceTree.Leafs.Count.Should().Be(targetTree.TargetLeafs.Count);
            for (var i = 0; i < sourceTree.Leafs.Count; i++)
            {
                targetTree.TargetLeafs[i].TargetTree.Should().Be(targetTree);
                targetTree.TargetLeafs[i].OriginId.Should().Be(sourceTree.Leafs.ElementAt(i).Id);
            }
        }

        #endregion

        #region Asserts

        private static void AssertRoot(SourceRoot sourceRoot, TargetRoot targetRoot)
        {
            targetRoot.OriginRoot.Should().Be(sourceRoot);
            targetRoot.Name.Should().Be(sourceRoot.Name);
            targetRoot.RootId.Should().Be(sourceRoot.Id);
            targetRoot.Id.Should().Be(sourceRoot.Id);
            targetRoot.TargetId.Should().Be(sourceRoot.Id);
            targetRoot.NumberSourceDefault.Should().NotBe(sourceRoot.NumberSourceDefault);
            targetRoot.NumberSourceNotDefault.Should().Be(sourceRoot.NumberSourceNotDefault);
            targetRoot.NumberTargetDefault.Should().Be(sourceRoot.NumberTargetDefault);
            targetRoot.NumberTargetNotDefault.Should().NotBe(sourceRoot.NumberTargetNotDefault);
            targetRoot.NumberSourceLookedUp.Should().Be(sourceRoot.NumberSourceNotDefault);
            targetRoot.NumberSourceNotLookedUp.Should().Be(sourceRoot.NumberSourceNotDefault);

            // ToDo: Check is this a bug? see https://github.com/bbtsoftware/BBT.StructureTools/issues/65.
            // targetRoot.NumberLimitNotApplied.Should().Be(sourceRoot.NumberSourceDefault);
            targetRoot.NumberLimitApplied.Should().NotBe(sourceRoot.NumberSourceNotDefault);
            targetRoot.EnumValue.Should().Be(sourceRoot.EnumValue.Target);
            targetRoot.FilteredHist.Should().Be(sourceRoot.Tree.ExpectedFilteredHist);
            ((TargetDerivedLeaf)targetRoot.TargetLeaf).OriginId.Should().Be(((SourceDerivedLeaf)sourceRoot.Leaf).Id);

            targetRoot.TargetMasterData.OriginId.Should().Be(sourceRoot.ExpectedFilteredMasterData.Id);
            targetRoot.TargetMasterData.IsDefault.Should().Be(sourceRoot.ExpectedFilteredMasterData.IsDefault);
            targetRoot.FilteredMasterDataId.Should().Be(sourceRoot.ExpectedFilteredMasterData.Id);

            AssertTree(sourceRoot, targetRoot);
        }

        private static void AssertTree(SourceRoot sourceRoot, TargetRoot targetRoot)
        {
            var sourceTree = sourceRoot.Tree;
            var targetTree = targetRoot.TargetTree;

            targetRoot.TargetTreeId.Should().Be(targetTree.Id);
            targetRoot.OriginTreeId.Should().Be(sourceTree.Id);
            targetRoot.TreeId.Should().Be(sourceTree.Id);
            targetTree.TargetRoot.Should().Be(targetRoot);
            targetTree.OriginTree.Should().Be(sourceRoot.Tree);
            targetTree.TreeName.Should().Be(sourceTree.TreeName);
            targetTree.MasterDataId.Should().Be(sourceTree.MasterData.Id);
            targetTree.TemporalDataOriginId.Should().Be(sourceTree.ExpectedFilteredHist.Id);
            targetTree.BaseDataId.Should().Be(sourceTree.BaseDataId);

            AssertTreeLeafs(sourceTree, targetTree);
            AssertTreeHistLeafs(sourceTree, targetTree);
        }

        private static void AssertTreeLeafs(SourceTree sourceTree, TargetTree targetTree)
        {
            targetTree.TargetLeafs.Count.Should().Be(sourceTree.Leafs.Count);

            for (var i = 0; i < sourceTree.Leafs.Count; i++)
            {
                var targetLeaf = targetTree.TargetLeafs.ElementAt(i);
                targetLeaf.TargetTree.Should().Be(targetTree);
                AssertTreeLeaf(sourceTree.Leafs.ElementAt(i), targetLeaf);
            }
        }

        private static void AssertTreeLeaf(SourceTreeLeaf sourceLeaf, TargetTreeLeaf targetLeaf)
        {
            targetLeaf.OriginId.Should().Be(sourceLeaf.Id);
            targetLeaf.MasterDataId.Should().Be(sourceLeaf.MasterDatas.First().Id);
            targetLeaf.LeafName.Should().Be(sourceLeaf.LeafName);
            targetLeaf.Child.OriginId.Should().Be(sourceLeaf.Id);
        }

        private static void AssertTreeHistLeafs(SourceTree sourceTree, TargetTree targetTree)
        {
            var sourceTreeHistLeafs = sourceTree.Leafs.SelectMany(x => x.TreeHistLeafs).ToList();
            targetTree.TargetHistLeafs.Count.Should().Be(sourceTreeHistLeafs.Count);

            for (var i = 0; i < sourceTreeHistLeafs.Count; i++)
            {
                var targetHistLeaf = targetTree.TargetHistLeafs.ElementAt(i);
                targetHistLeaf.TargetTree.Should().Be(targetTree);
                AssertTreeHistLeaf(sourceTreeHistLeafs.ElementAt(i), targetHistLeaf);
            }
        }

        private static void AssertTreeHistLeaf(SourceTreeHistLeaf sourceHistLeaf, TargetTreeHistLeaf targetHistLeaf)
        {
            targetHistLeaf.OriginTreeHistLeafId.Should().Be(sourceHistLeaf.TreeHistLeafId);
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
