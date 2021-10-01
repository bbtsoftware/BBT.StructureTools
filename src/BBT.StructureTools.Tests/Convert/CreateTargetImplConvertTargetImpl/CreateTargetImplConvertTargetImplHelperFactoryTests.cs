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

    public class CreateTargetImplConvertTargetImplHelperFactoryTests
    {
        private static IKernel container;

        public CreateTargetImplConvertTargetImplHelperFactoryTests()
        {
            container = TestIocContainer.Initialize();
        }

        [Fact]
        public void GetConvertHelper_NoReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelper<,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelper<,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper();

            testCandidate.Should().BeOfType<CreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetImplConvertTargetImplHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
        }

        [Fact]
        public void GetConvertHelper_WithReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperReverse<,,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper(x => x.TargetTree);

            testCandidate.Should().BeOfType<CreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.Should().BeOfType<CreateTargetImplConvertTargetImplHelperReverse<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
        }
    }
}
