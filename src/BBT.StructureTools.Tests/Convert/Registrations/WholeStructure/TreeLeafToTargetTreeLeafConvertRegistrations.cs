﻿namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using System.Linq;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.Registrations;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class TreeLeafToTargetTreeLeafConvertRegistrations : IConvertRegistrations<SourceTreeLeaf, TargetTreeLeaf, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeafChild, TargetTreeLeafChild, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeLeafToTargetTreeLeafConvertRegistrations" /> class.
        /// </summary>
        public TreeLeafToTargetTreeLeafConvertRegistrations(
            ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeafChild, TargetTreeLeafChild, IForTest> convertHelperFactory)
        {
            convertHelperFactory.NotNull(nameof(convertHelperFactory));

            this.convertHelperFactory = convertHelperFactory;
        }

        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(IConvertRegistration<SourceTreeLeaf, TargetTreeLeaf> registrations)
        {
            registrations.NotNull(nameof(registrations));

            registrations
                .RegisterCopyAttribute(x => x, x => x.OriginLeaf)
                .RegisterCopyAttribute(x => x.Id, x => x.OriginId)
                .RegisterCopyAttribute(x => x.LeafName, x => x.LeafName)
                .RegisterConvertFromSourceOnDifferentLevels<MasterData, TargetTreeLeaf, IForTest>(
                    x => x.MasterDatas.Single(y => y.IsDefault))
                .RegisterCreateFromSourceWithReverseRelation(
                    x => x.Child,
                    this.convertHelperFactory.GetConvertHelper(x => x.TargetTreeLeaf));
        }
    }
}
