namespace BBT.StructureTools.Copy.Operation
{
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyOperationSubCopy<T> : ICopyOperation<T>
        where T : class
    {
        private readonly ICopier<T> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationSubCopy{T}"/> class.
        /// </summary>
        public CopyOperationSubCopy(ICopier<T> copier)
        {
            copier.NotNull(nameof(copier));

            this.copier = copier;
        }

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            this.copier.Copy(source, target, copyCallContext);
        }
    }
}