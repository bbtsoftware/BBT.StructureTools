namespace BBT.StructureTools.Tests.Convert
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CreateTargetConvertTargetHelperFactoryTests
    {
        private static IKernel container;

        public CreateTargetConvertTargetHelperFactoryTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void GetConvertHelper_NoReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetConvertTargetHelperFactory<,,>)).To(typeof(CreateTargetConvertTargetHelperFactory<,,>));
            container.Bind(typeof(ICreateTargetConvertTargetHelper<,,>)).To(typeof(CreateTargetConvertTargetHelper<,,>));

            var testCandidate = container.Get<ICreateTargetConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper();

            testCandidate.Should().BeOfType<CreateTargetConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
        }

        [Fact]
        public void GetConvertHelper_WithReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetConvertTargetHelperFactory<,,>)).To(typeof(CreateTargetConvertTargetHelperFactory<,,>));
            container.Bind(typeof(ICreateTargetConvertTargetHelper<,,,>)).To(typeof(CreateTargetConvertTargetHelper<,,,>));

            var testCandidate = container.Get<ICreateTargetConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper(x => x.TargetTree);

            testCandidate.Should().BeOfType<CreateTargetConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
        }
    }
}
