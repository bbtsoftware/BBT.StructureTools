namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericConvertPostProcessing<TSoureClass, TTargetClass> : IConvertPostProcessing<TSoureClass, TTargetClass>
        where TSoureClass : class
        where TTargetClass : class
    {
        private readonly Action<TSoureClass, TTargetClass> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPostProcessing{TSoureClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPostProcessing(Action<TSoureClass, TTargetClass> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPostProcessing(TSoureClass source, TTargetClass target)
        {
            this.action.Invoke(source, target);
        }
    }
}
