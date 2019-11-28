namespace BBT.StructureTools.Tests.TestTools.IoC
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Initialization;

    public class IocHandlerStrategyLocator<TStrategy> : IStrategyLocator<TStrategy>
    {
        public IEnumerable<TStrategy> GetAllStrategies()
        {
            return IocHandler.Instance.IocResolver.GetAllInstances<TStrategy>();
        }
    }
}
