// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyWithRelationAndTargetRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<IdDto, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToManyWithRelationAndTargetRegistrations" /> class.
        /// </summary>
        public CreateToManyWithRelationAndTargetRegistrations(
            ICreateTargetImplConvertTargetHelperFactory<IdDto, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory)
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
                (x, y) => CreateSourceTreeLeafDtos(x, y),
                x => x.TargetLeafs,
                (x, y) => y.RelationOnTarget,
                this.convertHelperFactory.GetConvertHelper(x => x.RelationOnTarget));
        }

        private static IEnumerable<IdDto> CreateSourceTreeLeafDtos(SourceTree source, TargetTree target)
        {
            var dtos = source.Leafs.Select(x => new IdDto()
            {
                Id = target.Id,
            });

            return dtos;
        }
    }
}
