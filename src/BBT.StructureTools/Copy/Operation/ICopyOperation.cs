// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    /// <summary>
    /// Strategy to copy the different attribute types.
    /// </summary>
    /// <typeparam name="T">Type of class to copy.</typeparam>
    public interface ICopyOperation<in T>
    {
        /// <summary>
        /// Copies a single element.
        /// </summary>
        void Copy(T source, T target, ICopyCallContext copyCallContext);
    }
}