// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// Interface to intercept the convert process.
    /// </summary>
    /// <typeparam name="TSoureClass">Type of source class.</typeparam>
    /// <typeparam name="TTargetClass">Type of target class.</typeparam>
    public interface IConvertInterception<in TSoureClass, in TTargetClass> : IBaseAdditionalProcessing
        where TSoureClass : class
        where TTargetClass : class
    {
        /// <summary>
        /// This method will called before the convert process of the model in the type parameter starts.
        /// </summary>
        /// <returns><code>True</code> if the model must not convert, otherwise. <code>False</code></returns>
        bool CallConverter(TSoureClass source);
    }
}
