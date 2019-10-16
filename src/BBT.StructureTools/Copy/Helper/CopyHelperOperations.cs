// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Operation;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    public class CopyHelperOperations<T> : ICopyOperation<T>
    {
        private readonly IEnumerable<ICopyOperation<T>> mRegisteredStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyHelperOperations{T}"/> class.
        /// </summary>
        internal CopyHelperOperations(IEnumerable<ICopyOperation<T>> aRegisteredStrategies)
        {
            aRegisteredStrategies.Should().NotBeNull();

            this.mRegisteredStrategies = aRegisteredStrategies;
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            foreach (var lRegisteredStrategy in this.mRegisteredStrategies)
            {
                lRegisteredStrategy.Copy(source, target, copyCallContext);
            }
        }
    }
}