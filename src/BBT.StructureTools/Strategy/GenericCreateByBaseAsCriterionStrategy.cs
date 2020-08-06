// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Strategy
{
    using BBT.StrategyPattern;
    using BBT.StructureTools.Extension;

    /// <summary>
    /// Implementation of <see cref="ICreateByBaseAsCriterionStrategy{TBaseInterface, TInterface}"/>.
    /// </summary>
    /// <typeparam name="TBaseInterface">See above.</typeparam>
    /// <typeparam name="TCriterion">Implementation does return true on IsResponsible if <typeparamref name="TBaseInterface"/> is of type <typeparamref name="TCriterion"/>.</typeparam>
    /// <typeparam name="TBaseTargetInterface">Target base type interface.</typeparam>
    /// <typeparam name="TInterface">See above.</typeparam>
    /// <typeparam name="TImpl">Impl type of of <typeparamref name="TInterface"/>.</typeparam>
    public class GenericCreateByBaseAsCriterionStrategy<TBaseInterface, TCriterion, TBaseTargetInterface, TInterface, TImpl> : ICreateByBaseAsCriterionStrategy<TBaseInterface, TBaseTargetInterface>
        where TImpl : TInterface, new()
        where TInterface : class, TBaseTargetInterface
    {
        private readonly IInstanceCreator<TInterface, TImpl> instanceCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCreateByBaseAsCriterionStrategy{TBaseInterface, TCriterion, TConcreteInterface, TInterface, TImpl}"/> class.
        /// </summary>
        public GenericCreateByBaseAsCriterionStrategy(IInstanceCreator<TInterface, TImpl> instanceCreator)
        {
            StructureToolsArgumentChecks.NotNull(instanceCreator, nameof(instanceCreator));

            this.instanceCreator = instanceCreator;
        }

        /// <inheritdoc/>
        public TBaseTargetInterface CreateInstance()
        {
            var instance = this.instanceCreator.Create();
            return instance;
        }

        /// <inheritdoc/>
        public bool IsResponsible(TBaseInterface criterion)
        {
            return criterion is TCriterion;
        }
    }
}
