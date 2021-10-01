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

    public class CreateTargetImplConvertTargetImplHelperTests
    {
        private static IKernel container;

        public CreateTargetImplConvertTargetImplHelperTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void CreateTarget_NoReverseRelation_CreateAndConvert()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelper<,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetImplHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceTreeLeaf();
            var target = testCandidate.CreateTarget(source, processings);

            target.Should().NotBeNull();
            target.OriginId.Should().Be(source.Id);
        }

        [Fact]
        public void CreateTarget_WithReverseRelation_CreateAndConvert()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperReverse<,,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetImplHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
            testCandidate.SetupReverseRelation(x => x.TargetTree);

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceTreeLeaf();
            var reverseRelation = new TargetTree();
            var target = testCandidate.CreateTarget(source, reverseRelation, processings);

            target.Should().NotBeNull();
            target.OriginId.Should().Be(source.Id);
            target.TargetTree.Should().Be(reverseRelation);
        }
    }
}
