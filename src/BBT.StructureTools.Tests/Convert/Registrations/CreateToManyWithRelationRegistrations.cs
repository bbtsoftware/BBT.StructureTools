// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyWithRelationRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToManyWithRelationRegistrations" /> class.
        /// </summary>
        public CreateToManyWithRelationRegistrations(
            IConvertHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory)
        {
            StructureToolsArgumentChecks.NotNull(convertHelperFactory, nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, TargetTree> aRegistrations)
        {
            StructureToolsArgumentChecks.NotNull(aRegistrations, nameof(aRegistrations));

            aRegistrations.RegisterCreateToManyWithRelation(
                x => x.Leafs,
                x => x.TargetLeafs,
                (x, y) => y.RelationOnTarget,
                this.convertHelperFactory.GetConvertHelper(x => x.RelationOnTarget));
        }
    }
}
