namespace BBT.StructureTools.Convert
{
    using System;

    /// <summary>
    /// Generic implementation of <see cref="IConvertInterception{TSoureClass, TTargetClass}"/>.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
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

        /// <summary>
        /// This method will called before the convert process of the model in the type parameter starts.
        /// </summary>
        /// <returns><code>True</code> if the model must not convert, otherwise. <code>False</code></returns>
        public bool CallConverter(TSoureClass source)
        {
            return this.func.Invoke(source);
        }
    }
}
