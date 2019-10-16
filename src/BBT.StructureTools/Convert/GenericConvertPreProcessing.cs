// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;

    /// <summary>
    /// Generic implementation of <see cref="IConvertPreProcessing{TSoureClass, TTargetClass}"/>.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public class GenericConvertPreProcessing<TSoureClass, TTargetClass>
        : IConvertPreProcessing<TSoureClass, TTargetClass>
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<TSoureClass, TTargetClass> mAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPreProcessing{TSoureClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPreProcessing(Action<TSoureClass, TTargetClass> aAction)
        {
            this.mAction = aAction;
        }

        /// <summary>
        /// This method will called at the beginning of a convert process.
        /// </summary>
        public void DoPreProcessing(
            TSoureClass source,
            TTargetClass target)
        {
            this.mAction.Invoke(source, target);
        }
    }
}
