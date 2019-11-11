namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericConvertPreProcessing<TSourceClass, TTargetClass>
        : IConvertPreProcessing<TSourceClass, TTargetClass>
        where TSourceClass : class
        where TTargetClass : class
    {
        private readonly Action<TSourceClass, TTargetClass> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPreProcessing{TSourceClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPreProcessing(Action<TSourceClass, TTargetClass> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public void DoPreProcessing(
            TSourceClass source,
            TTargetClass target)
        {
            this.action.Invoke(source, target);
        }
    }
}
