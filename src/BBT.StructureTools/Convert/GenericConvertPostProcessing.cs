// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;

    /// <summary>
    /// Generic implementation of <see cref="IConvertPostProcessing{TSoureClass, TTargetClass}"/>.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public class GenericConvertPostProcessing<TSoureClass, TTargetClass> : IConvertPostProcessing<TSoureClass, TTargetClass>
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<TSoureClass, TTargetClass> mAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConvertPostProcessing{TSoureClass, TTargetClass}"/> class.
        /// </summary>
        public GenericConvertPostProcessing(Action<TSoureClass, TTargetClass> aAction)
        {
            this.mAction = aAction;
        }

        /// <summary>
        /// See <see cref="IConvertPostProcessing{TSoureClass, TTargetClass}.DoPostProcessing"/>.
        /// </summary>
        public void DoPostProcessing(TSoureClass source, TTargetClass target)
        {
            this.mAction.Invoke(source, target);
        }
    }
}
