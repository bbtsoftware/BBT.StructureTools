// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyGenericRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToManyGenericRegistrations" /> class.
        /// </summary>
        public CreateToManyGenericRegistrations(
            IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory)
        {
            convertHelperFactory.NotNull(nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, TargetTree> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations
                .RegisterCreateToManyGeneric(
                    x => x.Leafs,
                    x => x.TargetLeafs,
                    this.convertHelperFactory.GetConvertHelper());
        }
    }
}
