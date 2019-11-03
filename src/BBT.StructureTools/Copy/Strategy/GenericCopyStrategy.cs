namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class GenericCopyStrategy<TBase, TDerived, TConcrete> : ICopyStrategy<TBase>
        where TBase : class
        where TDerived : class, TBase
        where TConcrete : class, TDerived, new()
    {
        private readonly IInstanceCreator<TDerived, TConcrete> creator;
        private readonly ICopy<TDerived> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyStrategy{TBase, TDerived, TConcrete}"/> class.
        /// </summary>
        public GenericCopyStrategy(
            IInstanceCreator<TDerived, TConcrete> creator,
            ICopy<TDerived> copier)
        {
            creator.Should().NotBeNull();
            copier.Should().NotBeNull();

            this.creator = creator;
            this.copier = copier;
        }

        /// <inheritdoc/>
        public virtual void Copy(
            TBase source,
            TBase target,
            ICopyCallContext copyCallContext)
        {
            source.Should().BeAssignableTo<TDerived>();
            target.Should().BeAssignableTo<TDerived>();
            copyCallContext.Should().NotBeNull();

            this.copier.Copy(source as TDerived, target as TDerived, copyCallContext);
        }

        /// <inheritdoc/>
        public TBase Create()
        {
            var instance = this.creator.Create();
            return instance;
        }

        /// <inheritdoc/>
        public bool IsResponsible(TBase criterion)
        {
            return criterion is TConcrete;
        }
    }
}
