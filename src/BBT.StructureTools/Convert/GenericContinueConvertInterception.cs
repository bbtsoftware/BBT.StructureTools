namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericContinueConvertInterception<TSourceClass, TTargetClass> : IConvertInterception<TSourceClass, TTargetClass>
        where TSourceClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Func<TSourceClass, bool> func;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContinueConvertInterception{TSourceClass, TTargetClass}"/> class.
        /// </summary>
        public GenericContinueConvertInterception(Func<TSourceClass, bool> func)
        {
            this.func = func;
        }

        /// <inheritdoc/>
        public bool CallConverter(TSourceClass source)
        {
            return this.func.Invoke(source);
        }
    }
}
