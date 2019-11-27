namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.Intention;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using FluentAssertions;

    public class RootTargetRootConverterRegistrations : IConverterRegistrations<Root, TargetRoot, ITestConvertIntention>
    {
        private readonly IConvertHelperFactory<Tree, TargetTree, TargetTree, ITestConvertIntention> convertHelperFactory;

        public RootTargetRootConverterRegistrations(IConvertHelperFactory<Tree, TargetTree, TargetTree, ITestConvertIntention> convertHelperFactory)
        {
            this.convertHelperFactory = convertHelperFactory;
        }

        public void DoRegistrations(IConvertRegistration<Root, TargetRoot> registrations)
        {
            registrations.Should().NotBeNull();

            registrations
                .RegisterCopyAttribute(x => x.RootName, x => x.TargetRootName)
                .RegisterCreateToOneWithReverseRelation(x => x.Tree, x => x.TargetTree, this.convertHelperFactory.GetConvertHelper(x => x.TargetRoot));
        }
    }
}
