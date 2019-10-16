// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy.Processing;
    using FluentAssertions;

    /// <summary>
    /// Post processing which collects models which were processed by the copy.
    /// </summary>
    /// <typeparam name="TClassToCopy">Source type.</typeparam>
    public class ModelCollectingCopyPostProcessing<TClassToCopy> : ICopyPostProcessing<TClassToCopy>
        where TClassToCopy : class
    {
        private readonly List<TClassToCopy> mCollectedTargetInstances;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelCollectingCopyPostProcessing{TClassToCopy}"/> class.
        /// </summary>
        public ModelCollectingCopyPostProcessing()
        {
            this.mCollectedTargetInstances = new List<TClassToCopy>();
        }

        /// <summary>
        /// Gets a list of the collected objects.
        /// </summary>
        public IEnumerable<TClassToCopy> CollectedObjects => this.mCollectedTargetInstances;

        /// <summary>
        /// <see cref="ICopyPostProcessing{TClassToCopy}"/>.
        /// </summary>
        public void DoPostProcessing(TClassToCopy source, TClassToCopy target)
        {
            target.Should().NotBeNull();

            this.mCollectedTargetInstances.Add(target);
        }
    }
}
