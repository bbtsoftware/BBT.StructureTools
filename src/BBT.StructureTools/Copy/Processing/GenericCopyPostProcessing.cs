// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Processing
{
    using System;

    /// <summary>
    /// Generic implementation of <see cref="ICopyPostProcessing{TClassToCopy}"/>.
    /// </summary>
    /// <typeparam name="TClassToCopy">Type of to copied class.</typeparam>
    public class GenericCopyPostProcessing<TClassToCopy> : ICopyPostProcessing<TClassToCopy>
        where TClassToCopy : class
    {
        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<TClassToCopy, TClassToCopy> mAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyPostProcessing{TClassToCopy}"/> class.
        /// </summary>
        public GenericCopyPostProcessing(Action<TClassToCopy, TClassToCopy> aAction)
        {
            this.mAction = aAction;
        }

        /// <summary>
        /// This method will called at the end of a copy process.
        /// </summary>
        public void DoPostProcessing(TClassToCopy source, TClassToCopy target)
        {
            this.mAction.Invoke(source, target);
        }
    }
}
