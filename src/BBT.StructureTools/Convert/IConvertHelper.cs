namespace BBT.StructureTools.Convert
{
    using System.Collections.Generic;

    /// <summary>
    /// Helper for the convert implementations.
    /// </summary>
    internal interface IConvertHelper
    {
        /// <summary>
        /// Start the convert pre process it it's needed.
        /// </summary>
        /// <typeparam name="TSourceClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        void DoConvertPreProcessing<TSourceClass, TTargetClass>(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class;

        /// <summary>
        /// Start the convert post process it it's needed.
        /// </summary>
        /// <typeparam name="TSourceClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        void DoConvertPostProcessing<TSourceClass, TTargetClass>(
            TSourceClass source,
            TTargetClass target,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class;

        /// <summary>
        /// Evaluate the implementation of <see cref="IConvertInterception{TSourceClass,TTargetClass}"/>
        /// and return the result.
        /// </summary>
        /// <typeparam name="TSourceClass">Type of source class.</typeparam>
        /// <typeparam name="TTargetClass">Type of target class.</typeparam>
        bool ContinueConvertProcess<TSourceClass, TTargetClass>(
            TSourceClass source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
            where TSourceClass : class
            where TTargetClass : class;
    }
}