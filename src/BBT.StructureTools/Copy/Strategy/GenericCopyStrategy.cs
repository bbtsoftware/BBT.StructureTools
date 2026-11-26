namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Implementation of the <see cref="ICopyStrategy{TBase}"/> class.
    /// </summary>
    /// <typeparam name="TBase">Base type of the instances to copy.</typeparam>
    /// <typeparam name="TDerived">Type of the instances to copy, inherits from TBase.</typeparam>
    /// <typeparam name="TConcrete">Concrete implementation of the instances to copy, inherits from TDerived.</typeparam>
    public class GenericCopyStrategy<TBase, TDerived, TConcrete> : ICopyStrategy<TBase>
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
            var sourceCastConcrete = source.IsOfType<TDerived>(nameof(source));
            var targetCastConcrete = target.IsOfType<TDerived>(nameof(target));
            copyCallContext.NotNull(nameof(copyCallContext));

            this.copier.Copy(sourceCastConcrete, targetCastConcrete, copyCallContext);
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
