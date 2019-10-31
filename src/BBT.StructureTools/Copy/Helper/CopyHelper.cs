// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyHelper"/>.
    /// </summary>
    public class CopyHelper : ICopyHelper
    {
        /// <summary>
        /// See <see cref="ICopyHelper.DoCopyPostProcessing{T}"/>.
        /// </summary>
        /// <typeparam name="TClassToCopy">Class to copy.</typeparam>
        public void DoCopyPostProcessing<TClassToCopy>(
            TClassToCopy source,
            TClassToCopy target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TClassToCopy : class
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            additionalProcessings.OfType<ICopyPostProcessing<TClassToCopy>>()
                                 .ToList()
                                 .ForEach(x => x.DoPostProcessing(source, target));
        }
    }
}
