namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;

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
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            additionalProcessings.OfType<ICopyPostProcessing<TClassToCopy>>()
                                 .ToList()
                                 .ForEach(x => x.DoPostProcessing(source, target));
        }
    }
}
