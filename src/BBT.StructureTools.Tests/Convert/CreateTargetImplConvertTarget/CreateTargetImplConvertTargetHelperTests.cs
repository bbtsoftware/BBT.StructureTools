namespace BBT.StructureTools.Tests.Convert
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CreateTargetImplConvertTargetHelperTests
    {
        private static IKernel container;

        public CreateTargetImplConvertTargetHelperTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void CreateTarget_NoReverseRelation_CreateAndConvert()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceTreeLeaf();
            var target = testCandidate.Create(source);
            testCandidate.Convert(source, target, processings);

            target.Should().NotBeNull();
            target.OriginId.Should().Be(source.Id);
        }

        [Fact]
        public void CreateTarget_WithReverseRelation_CreateAndConvert()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
            testCandidate.SetupReverseRelation(x => x.TargetTree);

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceTreeLeaf();
            var reverseRelation = new TargetTree();
            var target = testCandidate.Create(source, reverseRelation);
            testCandidate.Convert(source, target, processings);

            target.Should().NotBeNull();
            target.OriginId.Should().Be(source.Id);
            target.TargetTree.Should().Be(reverseRelation);
        }
    }
}
