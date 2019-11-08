﻿namespace BBT.StructureTools.Copy.Processing
{
    using System;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class GenericContinueCopyInterception<TType> : IGenericContinueCopyInterception<TType>
        where TType : class
    {
        private readonly Func<TType, bool> shallCopyFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericContinueCopyInterception{TType}"/> class.
        /// </summary>
        public GenericContinueCopyInterception(Func<TType, bool> shallCopyFunc)
        {
            shallCopyFunc.NotNull(nameof(shallCopyFunc));

            this.shallCopyFunc = shallCopyFunc;
        }

        /// <inheritdoc/>
        public bool ShallCopy(TType obj)
        {
            obj.NotNull(nameof(obj));

            return this.shallCopyFunc.Invoke(obj);
        }
    }
}