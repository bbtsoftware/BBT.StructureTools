namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CopyHelper : ICopyHelper
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

            foreach (var additionalProcessing in additionalProcessings.OfType<ICopyPostProcessing<TClassToCopy>>())
            {
                additionalProcessing.DoPostProcessing(source, target);
            }
        }
    }
}
