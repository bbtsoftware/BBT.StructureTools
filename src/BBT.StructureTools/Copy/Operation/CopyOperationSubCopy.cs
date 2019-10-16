// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using BBT.StructureTools.Copy;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyOperation{T}"/>.
    /// </summary>
    /// <typeparam name="T">Owner of the attribute to copy.</typeparam>
    public class CopyOperationSubCopy<T> : ICopyOperation<T>
        where T : class
    {
        private readonly ICopy<T> mCopier;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationSubCopy{T}"/> class.
        /// </summary>
        public CopyOperationSubCopy(ICopy<T> aCopier)
        {
            aCopier.Should().NotBeNull();

            this.mCopier = aCopier;
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            this.mCopier.Copy(source, target, copyCallContext);
        }
    }
}