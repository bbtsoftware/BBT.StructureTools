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

    public class CreateTargetImplConvertTargetHelperFactoryTests
    {
        private static IKernel container;

        public CreateTargetImplConvertTargetHelperFactoryTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void GetConvertHelper_NoReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper();

            testCandidate.Should().BeOfType<CreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetImplConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
        }

        [Fact]
        public void GetConvertHelper_WithReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetHelper<,,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper(x => x.TargetTree);

            testCandidate.Should().BeOfType<CreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetImplConvertTargetHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
        }
    }
}
