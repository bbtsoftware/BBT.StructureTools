// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Strategy
{
    using System;
    using BBT.StrategyPattern;
    using FluentAssertions;

    /// <summary>
    /// Implementation of <see cref="ICreateInstanceOfTypeStrategy{TBaseTypeIntf}"/>.
    /// </summary>
    /// <typeparam name="TBaseTypeIntf">Interface type of the abstract base.</typeparam>
    /// <typeparam name="TConcreteTypeIntf">Interface derived from TBaseTypeIntf.</typeparam>
    /// <typeparam name="TConcreteTypeImpl">Implementation type for TConcreteTypeIntf.</typeparam>
    public class CreateInstanceOfTypeStrategy<TBaseTypeIntf, TConcreteTypeIntf, TConcreteTypeImpl> : ICreateInstanceOfTypeStrategy<TBaseTypeIntf>
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

        /// <summary>
        /// Gets the ... see <see cref="ICreateInstanceOfTypeStrategy{TBaseTypeIntf}.ConcreteIntfType"/>.
        /// </summary>
        public Type ConcreteIntfType => typeof(TConcreteTypeIntf);

        /// <summary>
        /// Gets the ... see <see cref="ICreateInstanceOfTypeStrategy{TBaseTypeIntf}.ConcreteImplType"/>.
        /// </summary>
        public Type ConcreteImplType => typeof(TConcreteTypeImpl);

        /// <summary>
        /// See <see cref="ICreateInstanceOfTypeStrategy{TBaseTypeIntf}.CreateInstance"/>.
        /// </summary>
        public TBaseTypeIntf CreateInstance() => this.instanceCreator.Create();

        /// <summary>
        /// See <see cref="IGenericStrategy{TBaseTypeIntf}.IsResponsible"/>.
        /// </summary>
        public bool IsResponsible(Type criterion) => criterion == typeof(TConcreteTypeIntf) || criterion == typeof(TConcreteTypeImpl);
    }
}
