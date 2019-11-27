﻿namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools.IoC;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [TestFixtureSource(typeof(IocTestFixtureSource), "IocContainers")]
    public class CopyOperationRegisterCreateFromFactoryTests
    {
        private readonly ICopy<TestClass> testcandidate;

        public CopyOperationRegisterCreateFromFactoryTests(IIocContainer iocContainer)
        {
            iocContainer.Initialize();

            iocContainer.RegisterSingleton(typeof(ICopyRegistrations<TestClass>), typeof(TestClassCopyRegistrations));
            iocContainer.RegisterSingleton(typeof(ITestFactory), typeof(TestFactory));

            this.testcandidate = iocContainer.GetInstance<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var source = new TestClass { Value1 = 45 };
            var target = new TestClass();

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value1.Should().Be(888);
            target.Value2.Should().Be(999);
        }

        /// <summary>
        /// An interface defining a Factory for test values - Used with RegisterInlineIocFactoryProcessing.
        /// </summary>
        private interface ITestFactory
        {
            /// <summary>
            /// Gets a static integer value.
            /// </summary>
            int Value2 { get; }

            /// <summary>
            /// Returns a integer value.
            /// </summary>
            /// <returns>a static integer value.</returns>
            int GetValue1();
        }

        private class TestClass
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestFactory : ITestFactory
        {
            public int Value2 => 999;

            public int GetValue1()
            {
                return 888;
            }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterCreateFromFactory<ITestFactory, int>(x => x.Value1, x => x.GetValue1())
                    .RegisterCreateFromFactory<ITestFactory, int>(x => x.Value2, x => x.Value2);
            }
        }
    }
}