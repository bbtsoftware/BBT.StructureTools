// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;
    using FluentAssertions;

    /// <summary>
    /// Post processing which collects models which were processed by the convert.
    /// </summary>
    /// <typeparam name="TSource">Source type.</typeparam>
    /// <typeparam name="TTarget">Target type.</typeparam>
    public class ModelCollectingConvertPostProcessing<TSource, TTarget> : IConvertPostProcessing<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private readonly List<TTarget> mCollectedTargetInstances;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelCollectingConvertPostProcessing{TSource, TTarget}"/> class.
        /// </summary>
        public ModelCollectingConvertPostProcessing()
        {
            this.mCollectedTargetInstances = new List<TTarget>();
        }

        /// <summary>
        /// Gets a list of the collected objects.
        /// </summary>
        public IEnumerable<TTarget> CollectedObjects => this.mCollectedTargetInstances;

        /// <summary>
        /// <see cref="IConvertPostProcessing{TSoureClass, TTargetClass}"/>.
        /// </summary>
        public void DoPostProcessing(TSource source, TTarget target)
        {
            target.Should().NotBeNull();

            this.mCollectedTargetInstances.Add(target);
        }
    }
}
