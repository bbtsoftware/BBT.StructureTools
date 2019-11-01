namespace BBT.StructureTools.Copy.Operation
{
    using System.Collections.ObjectModel;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <inheritdoc/>
    public class CopyOperationPostProcessing<T> : ICopyOperationPostProcessing<T>
        where T : class
    {
        private Collection<IBaseAdditionalProcessing> additionalProcessings;

        /// <inheritdoc/>
        public void Copy(T source, T aTarget, ICopyCallContext copyCallContext)
        {
            copyCallContext.Should().NotBeNull();
            this.additionalProcessings.Should().NotBeNull();

            copyCallContext.AdditionalProcessings.AddRangeToMe(this.additionalProcessings);
        }

        /// <inheritdoc/>
        public void Initialize(Collection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            this.additionalProcessings = additionalProcessings;
        }
    }
}