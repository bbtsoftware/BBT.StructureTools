// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToOneWithRelationAndTargetRegistrations : IConvertRegistrations<SourceRoot, TargetRoot, IForTest>
    {
        private readonly IConvertHelperFactory<IdDto, TargetTree, TargetTree, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToOneWithRelationAndTargetRegistrations" /> class.
        /// </summary>
        public CreateToOneWithRelationAndTargetRegistrations(
            IConvertHelperFactory<IdDto, TargetTree, TargetTree, IForTest> convertHelperFactory)
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
                (x, y) => GetIdDto(y),
                x => x.TargetTree,
                (x, y) => y.RelationOnTarget,
                this.convertHelperFactory.GetConvertHelper(x => x.RelationOnTarget));
        }

        private static IdDto GetIdDto(TargetRoot target)
        {
            var dto = new IdDto()
            {
                Id = target.Id,
            };

            return dto;
        }
    }
}
