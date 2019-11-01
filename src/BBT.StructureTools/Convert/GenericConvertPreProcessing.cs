namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericConvertPreProcessing<TSoureClass, TTargetClass>
        : IConvertPreProcessing<TSoureClass, TTargetClass>
        where TSoureClass : class
        where TTargetClass : class
    {
        private readonly Action<TSoureClass, TTargetClass> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPreProcessing{TSoureClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPreProcessing(Action<TSoureClass, TTargetClass> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPreProcessing(
            TSoureClass source,
            TTargetClass target)
        {
            this.action.Invoke(source, target);
        }
    }
}
