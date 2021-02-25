namespace BBT.StructureTools.Copy.Operation
{
    using System.Collections.ObjectModel;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationPostProcessing<T> : ICopyOperationPostProcessing<T>
        where T : class
    {
        private Collection<IBaseAdditionalProcessing> additionalProcessings;

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            copyCallContext.NotNull(nameof(copyCallContext));
            this.additionalProcessings.NotNull(nameof(this.additionalProcessings));

            copyCallContext.AdditionalProcessings.AddRangeToMe(this.additionalProcessings);
        }

        /// <inheritdoc/>
        public void Initialize(Collection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.NotNull(nameof(additionalProcessings));
            this.additionalProcessings = additionalProcessings;
        }
    }
}