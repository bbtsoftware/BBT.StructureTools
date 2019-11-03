namespace BBT.StructureTools.Strategy
{
    using System;
    using BBT.StrategyPattern;
    using FluentAssertions;

    /// <inheritdoc/>
    internal class CreateInstanceOfTypeStrategy<TBaseTypeIntf, TConcreteTypeIntf, TConcreteTypeImpl> : ICreateInstanceOfTypeStrategy<TBaseTypeIntf>
        where TBaseTypeIntf : class
        where TConcreteTypeIntf : class, TBaseTypeIntf
        where TConcreteTypeImpl : TConcreteTypeIntf, new()
    {
        private readonly IInstanceCreator<TConcreteTypeIntf, TConcreteTypeImpl> instanceCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateInstanceOfTypeStrategy{TBaseTypeIntf, TConcreteTypeIntf, TConcreteTypeImpl}"/> class.
        /// </summary>
        public CreateInstanceOfTypeStrategy(IInstanceCreator<TConcreteTypeIntf, TConcreteTypeImpl> instanceCreator)
        {
            instanceCreator.Should().NotBeNull();

            this.instanceCreator = instanceCreator;
        }

        /// <inheritdoc/>
        public Type ConcreteIntfType => typeof(TConcreteTypeIntf);

        /// <inheritdoc/>
        public Type ConcreteImplType => typeof(TConcreteTypeImpl);

        /// <inheritdoc/>
        public TBaseTypeIntf CreateInstance() => this.instanceCreator.Create();

        /// <inheritdoc/>
        public bool IsResponsible(Type criterion) => criterion == typeof(TConcreteTypeIntf) || criterion == typeof(TConcreteTypeImpl);
    }
}
