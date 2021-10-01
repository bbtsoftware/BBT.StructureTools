namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Extensions.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToManyWithReverseRelationRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly ICreateTargetImplConvertTargetHelperFactory<SourceTreeLeaf, TargetTreeLeaf, TargetTreeLeaf, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToManyWithReverseRelationRegistrations" /> class.
        /// </summary>
        public CreateToManyWithReverseRelationRegistrations(
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

            registrations.RegisterCreateToManyWithReverseRelation(
                x => x.Leafs,
                x => x.TargetLeafs,
                this.convertHelperFactory.GetConvertHelper(x => x.TargetTree));
        }
    }
}
