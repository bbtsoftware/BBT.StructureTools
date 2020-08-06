namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CopyCallContext : ICopyCallContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCallContext"/> class.
        /// </summary>
        public CopyCallContext()
        {
            this.AdditionalProcessings = new List<IBaseAdditionalProcessing>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCallContext"/> class.
        /// TODO: remove this constructor.
        /// </summary>
        public CopyCallContext(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            StructureToolsArgumentChecks.NotNull(additionalProcessings, nameof(additionalProcessings));

            this.AdditionalProcessings = additionalProcessings;
        }

        /// <inheritdoc/>
        public ICollection<IBaseAdditionalProcessing> AdditionalProcessings { get; }
    }
}
