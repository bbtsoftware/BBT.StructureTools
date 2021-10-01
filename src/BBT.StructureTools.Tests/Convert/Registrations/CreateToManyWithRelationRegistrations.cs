// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyWithRelationRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToManyWithRelationRegistrations" /> class.
        /// </summary>
        public CreateToManyWithRelationRegistrations(
            ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory)
        {
            convertHelperFactory.NotNull(nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTree, TargetTree> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations.RegisterCreateToManyWithRelation(
                x => x.Leafs,
                x => x.TargetLeafs,
                (x, y) => y.RelationOnTarget,
                this.convertHelperFactory.GetConvertHelper(x => x.RelationOnTarget));
        }
    }
}
