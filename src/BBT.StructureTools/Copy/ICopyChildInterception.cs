// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy
{
    /// <summary>
    /// Interface to intercept the copy process.
    /// </summary>
    /// <typeparam name="TClassToCopy">Type of to copied class.</typeparam>
    public interface ICopyChildInterception<in TClassToCopy> : IBaseAdditionalProcessing
        where TClassToCopy : class
    {
        /// <summary>
        /// This method will called before the copy process of the model in the type parameter starts.
        /// </summary>
        /// <returns><code>True</code> if the model must not copy, otherwise. <code>False</code></returns>
        bool CallCopyChild(TClassToCopy source, TClassToCopy target);
    }
}
