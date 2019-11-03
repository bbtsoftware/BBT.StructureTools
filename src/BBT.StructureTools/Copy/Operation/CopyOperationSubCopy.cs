namespace BBT.StructureTools.Copy.Operation
{
    using BBT.StructureTools.Copy;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class CopyOperationSubCopy<T> : ICopyOperation<T>
        where T : class
    {
        private readonly ICopy<T> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationSubCopy{T}"/> class.
        /// </summary>
        public CopyOperationSubCopy(ICopy<T> copier)
        {
            copier.Should().NotBeNull();

            this.copier = copier;
        }

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            this.copier.Copy(source, target, copyCallContext);
        }
    }
}