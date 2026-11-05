using System.Collections.Generic;

namespace BBT.StructureTools.Copy
{
    /// <summary>
    /// Provides copy functionality.
    /// </summary>
    public interface ICopyHelper
    {
        /// <summary>
        /// Start the copy post process it it's needed.
        /// </summary>
        /// <typeparam name="TClassToCopy">Class to copy.</typeparam>
        void DoCopyPostProcessing<TClassToCopy>(
            TClassToCopy source,
            TClassToCopy target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TClassToCopy : class;
    }
}
