namespace BBT.StructureTools.Copy.Processing
{
    using System;
    using BBT.StructureTools.Extension;

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
            shallCopyFunc.NotNull(nameof(shallCopyFunc));
            this.shallCopyFunc = shallCopyFunc;
        }

        /// <summary>
        /// <see cref="IGenericContinueCopyInterception{TType}.ShallCopy"/>.
        /// </summary>
        public bool ShallCopy(TType @object)
        {
            @object.NotNull(nameof(@object));

            return this.shallCopyFunc.Invoke(@object);
        }
    }
}