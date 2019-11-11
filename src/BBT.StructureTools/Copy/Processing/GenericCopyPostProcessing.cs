namespace BBT.StructureTools.Copy.Processing
{
    using System;

    /// <inheritdoc/>
    public class GenericCopyPostProcessing<TClassToCopy> : ICopyPostProcessing<TClassToCopy>
        where TClassToCopy : class
    {
        private readonly Action<TClassToCopy, TClassToCopy> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyPostProcessing{TClassToCopy}"/> class.
        /// </summary>
        public GenericCopyPostProcessing(Action<TClassToCopy, TClassToCopy> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPostProcessing(TClassToCopy source, TClassToCopy target)
        {
            this.action.Invoke(source, target);
        }
    }
}
