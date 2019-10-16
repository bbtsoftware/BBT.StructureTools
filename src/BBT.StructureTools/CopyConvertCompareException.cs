// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// a general copy convert compare exception.
    /// </summary>
    [Serializable]
    public class CopyConvertCompareException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyConvertCompareException"/> class.
        /// </summary>
        public CopyConvertCompareException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyConvertCompareException"/> class.
        /// </summary>
        /// <param name="aMessage">A message that describes the error.</param>
        public CopyConvertCompareException(string aMessage)
            : base(aMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyConvertCompareException"/> class.
        /// </summary>
        /// <param name="aMessage">The error message that explains the reason for the exception.</param>
        /// <param name="aInnerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public CopyConvertCompareException(string aMessage, Exception aInnerException)
            : base(aMessage, aInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyConvertCompareException"/> class.
        /// </summary>
        /// <param name="aInfo">The object that holds the serialized object data.</param>
        /// <param name="aContext">The contextual information about the source or destination.</param>
        protected CopyConvertCompareException(SerializationInfo aInfo, StreamingContext aContext)
            : base(aInfo, aContext)
        {
        }
    }
}
