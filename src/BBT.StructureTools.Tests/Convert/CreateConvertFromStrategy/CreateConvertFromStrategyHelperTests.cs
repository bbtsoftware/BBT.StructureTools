namespace BBT.StructureTools.Tests.Convert
{
    using System;
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.Registrations.WholeStructure;
    using BBT.StructureTools.Tests.Convert.TestData;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CreateConvertFromStrategyHelperTests
    {
        private static IKernel container;

        public CreateConvertFromStrategyHelperTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void CreateTarget_NoReverseRelation_CreateAndConvert()
        {
            container.Bind(typeof(ICreateConvertFromStrategyHelper<,,>)).To(typeof(CreateConvertFromStrategyHelper<,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));
            container.Bind<ICreateConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest>>().To<CreateConvertStrategy<SourceBaseLeaf, SourceDerivedLeaf, TargetBaseLeaf, TargetDerivedLeaf, TargetDerivedLeaf, IForTest>>();
            container.Bind<IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>>().To<DerivedLeafToTargetDerivedLeafConvertRegistrations>();

            var testCandidate = container.Get<ICreateConvertFromStrategyHelper<SourceBaseLeaf, TargetBaseLeaf, IForTest>>();

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceDerivedLeaf();
            var target = testCandidate.Create(source);
            testCandidate.Convert(source, target, processings);

            target.Should().NotBeNull();
            target.Should().BeOfType<TargetDerivedLeaf>();
            ((TargetDerivedLeaf)target).OriginId.Should().Be(source.Id);
        }

        [Fact]
        public void CreateTarget_WithReverseRelation_CreateAndConvert()
        {
            container.Bind(typeof(ICreateConvertFromStrategyHelper<,,,>)).To(typeof(CreateConvertFromStrategyHelperReverse<,,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));
            container.Bind<ICreateConvertStrategy<SourceBaseLeaf, TargetBaseLeaf, IForTest>>().To<CreateConvertStrategy<SourceBaseLeaf, SourceDerivedLeaf, TargetBaseLeaf, TargetDerivedLeaf, TargetDerivedLeaf, IForTest>>();
            container.Bind<IConvertRegistrations<SourceDerivedLeaf, TargetDerivedLeaf, IForTest>>().To<DerivedLeafToTargetDerivedLeafConvertRegistrations>();

            var testCandidate = container.Get<ICreateConvertFromStrategyHelper<SourceBaseLeaf, TargetBaseLeaf, TargetRoot, IForTest>>();
            testCandidate.SetupReverseRelation(x => x.TargetRoot);

            var processings = new List<IBaseAdditionalProcessing>();
            var source = new SourceDerivedLeaf();
            var reverseRelation = new TargetRoot();
            var target = testCandidate.Create(source, reverseRelation);
            testCandidate.Convert(source, target, processings);

            target.Should().NotBeNull();
            target.Should().BeOfType<TargetDerivedLeaf>();
            ((TargetDerivedLeaf)target).OriginId.Should().Be(source.Id);
            target.TargetRoot.Should().Be(reverseRelation);
        }
    }
}
