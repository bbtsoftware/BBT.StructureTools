// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;
    using FluentAssertions;

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
            creator.Should().NotBeNull();
            copier.Should().NotBeNull();

            this.creator = creator;
            this.copier = copier;
        }

        /// <summary>
        /// See <see cref="ICopyStrategy{T}.Copy"/>.
        /// </summary>
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

        /// <summary>
        /// See <see cref="ICopyStrategy{T}.Create"/>.
        /// </summary>
        public TBase Create()
        {
            var instance = this.creator.Create();
            return instance;
        }

        /// <summary>
        /// See <see cref="IGenericStrategy{T}.IsResponsible"/>.
        /// </summary>
        public bool IsResponsible(TBase aCriterion)
        {
            return aCriterion is TConcrete;
        }
    }
}
