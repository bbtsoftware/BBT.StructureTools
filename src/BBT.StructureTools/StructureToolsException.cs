namespace BBT.StructureTools
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// A general exception indicating that an unrecoverable error
    /// occured within the StructureTools library.
    /// </summary>
    [Serializable]
    public class StructureToolsException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureToolsException"/> class.
        /// </summary>
        public StructureToolsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureToolsException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public StructureToolsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureToolsException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public StructureToolsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureToolsException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected StructureToolsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
