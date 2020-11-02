namespace BBT.StructureTools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Post processing which collects models which were processed by the copy.
    /// </summary>
    /// <typeparam name="TClassToCopy">Source type.</typeparam>
    public class ModelCollectingCopyPostProcessing<TClassToCopy> : ICopyPostProcessing<TClassToCopy>
        where TClassToCopy : class
    {
        private readonly List<TClassToCopy> collectedTargetInstances;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelCollectingCopyPostProcessing{TClassToCopy}"/> class.
        /// </summary>
        public ModelCollectingCopyPostProcessing()
        {
            this.collectedTargetInstances = new List<TClassToCopy>();
        }

        /// <summary>
        /// Gets a list of the collected objects.
        /// </summary>
        public IEnumerable<TClassToCopy> CollectedObjects => this.collectedTargetInstances;

        /// <summary>
        /// <see cref="ICopyPostProcessing{TClassToCopy}"/>.
        /// </summary>
        public void DoPostProcessing(TClassToCopy source, TClassToCopy target)
        {
            target.NotNull(nameof(target));

            this.collectedTargetInstances.Add(target);
        }
    }
}
