namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Operation;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class Copier<T> : ICopy<T>
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
            copyRegistrations.Should().NotBeNull();
            copyHelper.Should().NotBeNull();
            factory.Should().NotBeNull();

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
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            additionalProcessings.Should().NotBeNull();

            this.Copy(source, target, new CopyCallContext(additionalProcessings));
        }

        /// <inheritdoc/>
        public void Copy(
            T source,
            T target,
            ICopyCallContext copyCallContext)
        {
            source.Should().NotBeNull();
            target.Should().NotBeNull();
            copyCallContext.Should().NotBeNull();

            this.operations.Copy(source, target, copyCallContext);

            this.copyHelper.DoCopyPostProcessing(source, target, copyCallContext.AdditionalProcessings);
        }
    }
}