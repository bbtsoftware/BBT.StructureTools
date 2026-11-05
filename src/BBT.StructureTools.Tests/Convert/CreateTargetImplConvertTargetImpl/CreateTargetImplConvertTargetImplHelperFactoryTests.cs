namespace BBT.StructureTools.Tests.Convert
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;
    using BBT.StructureTools.Tests.TestTools;
    using Ninject;
    using Shouldly;
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

            testCandidate.ShouldBeOfType<CreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.ShouldBeOfType<CreateTargetImplConvertTargetImplHelper<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
        }

        [Fact]
        public void GetConvertHelper_WithReverseRelation_GetsExpectedTypes()
        {
            container.Bind<IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>>().To<CopyLeafAttributeRegistrations>();
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelperFactory<,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperFactory<,,,>));
            container.Bind(typeof(ICreateTargetImplConvertTargetImplHelper<,,,,>)).To(typeof(CreateTargetImplConvertTargetImplHelperReverse<,,,,>));

            var testCandidate = container.Get<ICreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            var helper = testCandidate.GetConvertHelper(x => x.TargetTree);

            testCandidate.ShouldBeOfType<CreateTargetImplConvertTargetImplHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest>>();
            helper.ShouldBeOfType<CreateTargetImplConvertTargetImplHelperReverse<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, TargetTree, IForTest>>();
        }
    }
}
