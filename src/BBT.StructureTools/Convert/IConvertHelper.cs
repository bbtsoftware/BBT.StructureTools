namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;

    /// <summary>
    /// Helper for the convert implementations.
    /// </summary>
    public interface IConvertHelper
    {
        /// <summary>
        /// Start the convert pre process it it's needed.
        /// </summary>
        /// <typeparam name="TSoureClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        void DoConvertPreProcessing<TSoureClass, TTargetClass>(
            TSoureClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSoureClass : class
            where TTargetClass : class;

        /// <summary>
        /// Start the convert post process it it's needed.
        /// </summary>
        /// <typeparam name="TSoureClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        void DoConvertPostProcessing<TSoureClass, TTargetClass>(
            TSoureClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSoureClass : class
            where TTargetClass : class;

        /// <summary>
        /// Evaluate the implementation of <see cref="IConvertInterception{TSoureClass,TTargetClass}"/>
        /// and return the result.
        /// </summary>
        /// <typeparam name="TSoureClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        bool ContinueConvertProcess<TSoureClass, TTargetClass>(
            TSoureClass source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSoureClass : class
            where TTargetClass : class;
    }
}