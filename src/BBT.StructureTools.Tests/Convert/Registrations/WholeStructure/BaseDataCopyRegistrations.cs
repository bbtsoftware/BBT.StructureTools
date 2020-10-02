// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Tests.Convert.Registrations.WholeStructure
{
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Convert.TestData;

    /// <summary>
    /// Registrations for test purposes.
    /// </summary>
    public class BaseDataCopyRegistrations : ICopyRegistrations<BaseData>
    {
        /// <summary>
        /// See <see cref="IConvertRegistrations{TSource, TTarget, TConvertIntention}.DoRegistrations"/>.
        /// </summary>
        public void DoRegistrations(ICopyHelperRegistration<BaseData> aRegistrations)
        {
            aRegistrations.NotNull(nameof(aRegistrations));

            aRegistrations.RegisterAttribute(x => x.BaseDataId);
        }
    }
}
