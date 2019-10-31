// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System.Collections.ObjectModel;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyOperationPostProcessing{T}"/> for documentation.
    /// </summary>
    /// <typeparam name="T">see above.</typeparam>
    public class CopyOperationPostProcessing<T> : ICopyOperationPostProcessing<T>
        where T : class
    {
        private Collection<IBaseAdditionalProcessing> additionalProcessings;

        /// <summary>
        /// <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T aTarget, ICopyCallContext copyCallContext)
        {
            copyCallContext.Should().NotBeNull();
            this.additionalProcessings.Should().NotBeNull();

            copyCallContext.AdditionalProcessings.AddRangeToMe(this.additionalProcessings);
        }

        /// <summary>
        /// <see cref="ICopyOperationPostProcessing{T}"/>.Initialize.
        /// </summary>
        public void Initialize(Collection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            this.additionalProcessings = additionalProcessings;
        }
    }
}