namespace BBT.StructureTools.Copy.Processing
{
    using System;
    using FluentAssertions;

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
            shallCopyFunc.Should().NotBeNull();

            this.shallCopyFunc = shallCopyFunc;
        }

        /// <inheritdoc/>
        public bool Shalcopy(TType aObject)
        {
            aObject.Should().NotBeNull();

            return this.shallCopyFunc.Invoke(aObject);
        }
    }
}