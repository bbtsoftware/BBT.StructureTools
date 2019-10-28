using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace BBT.StructureTools.Tests.TestTools.IoC
{
    public class IocFixtureData
    {
        public IocFixtureData()
        {
            this.IocContainer = Setup.SetUpIocResolve();
        }

        public IUnityContainer IocContainer { get; }
    }
}
