namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class CopyHelperOperations<T> : ICopyOperation<T>
    {
        private readonly IEnumerable<ICopyOperation<T>> registeredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelperOperations{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is required and needs to be public because of the issue
        /// described in GH-17.
        /// </remarks>
        public CopyHelperOperations(IEnumerable<ICopyOperation<T>> registeredStrategies)
        {
            registeredStrategies.NotNull(nameof(registeredStrategies));

            this.registeredStrategies = registeredStrategies;
        }

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            foreach (var registeredStrategy in this.registeredStrategies)
            {
                registeredStrategy.Copy(source, target, copyCallContext);
            }
        }
    }
}