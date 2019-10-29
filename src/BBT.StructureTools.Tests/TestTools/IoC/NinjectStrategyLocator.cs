using System.Collections.Generic;
using BBT.StrategyPattern;
using BBT.StructureTools.Initialization;

namespace BBT.StructureTools.Tests.TestTools.IoC
{
    public class NinjectStrategyLocator<TStrategy> : IStrategyLocator<TStrategy>
    {
        public IEnumerable<TStrategy> GetAllStrategies()
        {
            return IocHandler.Instance.IocResolver.GetAllInstances<TStrategy>();
        }
    }
}
