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
    public class CreateToOneWithRelationRegistrations : IConvertRegistrations<SourceRoot, TargetRoot, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<SourceTree, TargetTree, TargetTree, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToOneWithRelationRegistrations" /> class.
        /// </summary>
        public CreateToOneWithRelationRegistrations(
            ICreateTargetImplConvertTargetHelperFactory<SourceTree, TargetTree, TargetTree, IForTest> convertHelperFactory)
        {
            convertHelperFactory.NotNull(nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceRoot, TargetRoot> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations.RegisterCreateToOneWithRelation(
                x => x.Tree,
                x => x.TargetTree,
                (x, y) => y.RelationOnTarget,
                this.convertHelperFactory.GetConvertHelper(x => x.RelationOnTarget));
        }
    }
}
