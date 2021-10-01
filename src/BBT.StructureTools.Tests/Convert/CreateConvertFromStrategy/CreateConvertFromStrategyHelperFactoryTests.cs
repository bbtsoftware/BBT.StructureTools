namespace BBT.StructureTools.Tests.Convert
{
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

    public class CreateConvertFromStrategyHelperFactoryTests
    {
        private static IKernel container;

        public CreateConvertFromStrategyHelperFactoryTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void GetConvertHelper_NoReverseRelation_GetsExpectedTypes()
        {
            container.Bind(typeof(ICreateConvertFromStrategyHelperFactory<,,>)).To(typeof(CreateConvertFromStrategyHelperFactory<,,>));
            container.Bind(typeof(ICreateConvertFromStrategyHelper<,,>)).To(typeof(CreateConvertFromStrategyHelper<,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));

            var testCandidate = container.Get<ICreateConvertFromStrategyHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper();

            testCandidate.Should().BeOfType<CreateConvertFromStrategyHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateConvertFromStrategyHelper<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
        }

        [Fact]
        public void GetConvertHelper_WithReverseRelation_GetsExpectedTypes()
        {
            container.Bind(typeof(ICreateConvertFromStrategyHelperFactory<,,>)).To(typeof(CreateConvertFromStrategyHelperFactory<,,>));
            container.Bind(typeof(ICreateConvertFromStrategyHelper<,,,>)).To(typeof(CreateConvertFromStrategyHelperReverse<,,,>));
            container.Bind(typeof(IGenericStrategyProvider<,>)).To(typeof(GenericStrategyProvider<,>));

            var testCandidate = container.Get<ICreateConvertFromStrategyHelperFactory<SourceBaseLeaf, TargetBaseLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper(x => x.TargetRoot);

            testCandidate.Should().BeOfType<CreateConvertFromStrategyHelperFactory<SourceBaseLeaf, TargetBaseLeaf, IForTest>>();
            helper.Should().BeOfType<CreateConvertFromStrategyHelperReverse<SourceBaseLeaf, TargetBaseLeaf, TargetRoot, IForTest>>();
        }
    }
}
