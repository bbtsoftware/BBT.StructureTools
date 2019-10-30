// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationRegisterCreateFromFactoryTests
    {
        private readonly ICopy<TestClass> testcandidate;

        public CopyOperationRegisterCreateFromFactoryTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ITestFactory>().To<TestFactory>();

            this.testcandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var lSource = new TestClass { Value1 = 45 };
            var lTarget = new TestClass();

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lTarget.Value1.Should().Be(888);
            lTarget.Value2.Should().Be(999);
        }

        /// <summary>
        /// An interface defining a factory for test values - Used with RegisterInlineIocFactoryProcessing.
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
            public void DoRegistrations(ICopyHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterCreateFromFactory<ITestFactory, int>(aX => aX.Value1, aX => aX.GetValue1())
                    .RegisterCreateFromFactory<ITestFactory, int>(aX => aX.Value2, aX => aX.Value2);
            }
        }
    }
}