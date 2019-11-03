namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class CopyHelper : ICopyHelper
    {
        /// <inheritdoc/>
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
