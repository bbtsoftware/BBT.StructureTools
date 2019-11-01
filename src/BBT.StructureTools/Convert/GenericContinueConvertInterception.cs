namespace BBT.StructureTools.Convert
{
    using System;

    /// <inheritdoc/>
    public class GenericContinueConvertInterception<TSoureClass, TTargetClass> : IConvertInterception<TSoureClass, TTargetClass>
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Func<TSoureClass, bool> func;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContinueConvertInterception{TSoureClass, TTargetClass}"/> class.
        /// </summary>
        public GenericContinueConvertInterception(Func<TSoureClass, bool> func)
        {
            this.func = func;
        }

        /// <inheritdoc/>
        public bool CallConverter(TSoureClass source)
        {
            return this.func.Invoke(source);
        }
    }
}
