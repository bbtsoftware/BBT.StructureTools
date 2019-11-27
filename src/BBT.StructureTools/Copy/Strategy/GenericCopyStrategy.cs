namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    internal class GenericCopyStrategy<TBase, TDerived, TConcrete> : ICopyStrategy<TBase>
        where TBase : class
        where TDerived : class, TBase
        where TConcrete : class, TDerived, new()
    {
        private readonly IInstanceCreator<TDerived, TConcrete> creator;
        private readonly ICopier<TDerived> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyStrategy{TBase, TDerived, TConcrete}"/> class.
        /// </summary>
        public GenericCopyStrategy(
            IInstanceCreator<TDerived, TConcrete> creator,
            ICopier<TDerived> copier)
        {
            creator.NotNull(nameof(creator));
            copier.NotNull(nameof(copier));

            this.creator = creator;
            this.copier = copier;
        }

        /// <inheritdoc/>
        public virtual void Copy(
            TBase source,
            TBase target,
            ICopyCallContext copyCallContext)
        {
            source.IsOfType<TDerived>(nameof(source));
            target.IsOfType<TDerived>(nameof(target));
            copyCallContext.NotNull(nameof(copyCallContext));

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
