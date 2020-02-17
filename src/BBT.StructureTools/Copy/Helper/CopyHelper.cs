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
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelper"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public CopyHelper()
        {
        }

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
