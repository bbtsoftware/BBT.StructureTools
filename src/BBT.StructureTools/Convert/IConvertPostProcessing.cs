// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to add some additional functions on the end of the convert process.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertPostProcessing<TSoureClass, TTargetClass> : IBaseAdditionalProcessing
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will be called at the end of a convert process.
        /// </summary>
        void DoPostProcessing(TSoureClass source, TTargetClass target);
    }
}
