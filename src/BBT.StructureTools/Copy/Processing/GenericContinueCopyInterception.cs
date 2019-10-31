// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Processing
{
    using System;
    using FluentAssertions;

    /// <summary>
    /// Implementation of <see cref="IGenericContinueCopyInterception{TType}"/>.
    /// </summary>
    /// <typeparam name="TType">type on which the interception applies.</typeparam>
    public class GenericContinueCopyInterception<TType> : IGenericContinueCopyInterception<TType>
        where TType : class
    {
        private readonly Func<TType, bool> shallCopyFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContinueCopyInterception{TType}"/> class.
        /// </summary>
        public GenericContinueCopyInterception(Func<TType, bool> shallCopyFunc)
        {
            shallCopyFunc.Should().NotBeNull();

            this.shallCopyFunc = shallCopyFunc;
        }

        /// <summary>
        /// <see cref="IGenericContinueCopyInterception{TType}.Shalcopy"/>.
        /// </summary>
        public bool Shalcopy(TType aObject)
        {
            aObject.Should().NotBeNull();

            return this.shallCopyFunc.Invoke(aObject);
        }
    }
}