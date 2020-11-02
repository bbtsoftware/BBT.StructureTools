// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class CreateToOneHistWithConditionRegistrations : IConvertRegistrations<SourceTree, TargetTree, IForTest>
    {
        private readonly IConvertHelperFactory<SourceTreeHist, TargetTreeHist, TargetTreeHist, IForTest> convertHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToOneHistWithConditionRegistrations" /> class.
        /// </summary>
        public CreateToOneHistWithConditionRegistrations(
            IConvertHelperFactory<SourceTreeHist, TargetTreeHist, TargetTreeHist, IForTest> convertHelperFactory)
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

            registrations.RegisterCreateToOneHistWithCondition<SourceTreeHist, TargetTreeHist, TargetTreeHist, TargetTree, ITemporalData, IForTest>(
                (x, y) => x.Hists,
                x => x.TargetHists,
                (x, y) => true,
                (x, y) => y.TargetRoot.ReferenceDate,
                this.convertHelperFactory.GetConvertHelper(x => x.TargetTree));
        }
    }
}
