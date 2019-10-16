// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="ICopyCallContext"/>.
    /// </summary>
    public class CopyCallContext : ICopyCallContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCallContext"/> class.
        /// </summary>
        public CopyCallContext()
        {
            this.AdditionalProcessings = new List<IBaseAdditionalProcessing>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCallContext"/> class.
        /// TODO: remove this constructor.
        /// </summary>
        public CopyCallContext(ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            additionalProcessings.Should().NotBeNull();

            this.AdditionalProcessings = additionalProcessings;
        }

        /// <summary>
        /// Gets... see <see cref="ICopyCallContext.AdditionalProcessings"/>.
        /// </summary>
        public ICollection<IBaseAdditionalProcessing> AdditionalProcessings { get; }
    }
}
