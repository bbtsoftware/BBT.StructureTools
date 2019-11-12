namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericConvertPostProcessing<TSourceClass, TTargetClass> : IConvertPostProcessing<TSourceClass, TTargetClass>
        where TSourceClass : class
        where TTargetClass : class
    {
        private readonly Action<TSourceClass, TTargetClass> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPostProcessing{TSourceClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPostProcessing(Action<TSourceClass, TTargetClass> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPostProcessing(TSourceClass source, TTargetClass target)
        {
            this.action.Invoke(source, target);
        }
    }
}
