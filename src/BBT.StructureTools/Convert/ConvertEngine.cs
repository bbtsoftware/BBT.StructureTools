// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    /// <summary>
    /// See <see cref="IConvertEngine{TSource,TTarget}"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public class ConvertEngine<TSource, TTarget> : IConvertEngine<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// See <see cref="IConvertEngine{TSource,TTarget}.StartRegistrations"/>.
        /// </summary>
        public IConvertRegistration<TSource, TTarget> StartRegistrations()
        {
            return new ConvertRegistration<TSource, TTarget>();
        }
    }
}
