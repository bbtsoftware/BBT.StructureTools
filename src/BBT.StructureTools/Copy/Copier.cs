namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Generic copy class.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    public class Copier<T> : ICopy<T>
        where T : class
    {
        private readonly ICopyHelper copyHelper;
        private readonly ICopyOperation<T> operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="Copier{T}" /> class.
        /// </summary>
        public Copier(
            ICopyRegistrations<T> copyRegistrations,
            ICopyHelper copyHelper,
            ICopyHelperRegistrationFactory factory)
        {
            copyRegistrations.NotNull(nameof(copyRegistrations));
            copyHelper.NotNull(nameof(copyHelper));
            factory.NotNull(nameof(factory));

            var registrations = factory.Create<T>();
            copyRegistrations.DoRegistrations(registrations);
            this.operations = registrations.EndRegistrations();

            this.copyHelper = copyHelper;
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            this.Copy(source, target, new CopyCallContext(additionalProcessings));
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            copyCallContext.NotNull(nameof(copyCallContext));

            this.operations.Copy(source, target, copyCallContext);

            this.copyHelper.DoCopyPostProcessing(source, target, copyCallContext.AdditionalProcessings);
        }
    }
}