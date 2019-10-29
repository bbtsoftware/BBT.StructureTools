using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.Intention;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using FluentAssertions;

namespace BBT.StructureTools.Tests.Convert.TestStructure.Registration
{
    public class RootTargetRootConvertRegistrations : IConvertRegistrations<Root, TargetRoot, ITestConvertIntention>
    {
        private readonly IConvertHelperFactory<Tree, TargetTree, TargetTree, ITestConvertIntention> convertHelperFactory;

        public RootTargetRootConvertRegistrations(IConvertHelperFactory<Tree, TargetTree, TargetTree, ITestConvertIntention> convertHelperFactory)
        {
            this.convertHelperFactory = convertHelperFactory;
        }

        public void DoRegistrations(IConvertRegistration<Root, TargetRoot> aRegistrations)
        {
            aRegistrations.Should().NotBeNull();

            aRegistrations
                .RegisterCopyAttribute(aX => aX.RootName, aX => aX.TargetRootName)
                .RegisterCreateToOneWithReverseRelation(aX => aX.Tree, aX => aX.TargetTree, this.convertHelperFactory.GetConvertHelper(aX => aX.TargetRoot));
        }
    }
}
