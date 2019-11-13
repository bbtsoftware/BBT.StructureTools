namespace BBT.StructureTools.Tests.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationRegisterCreateFromFactoryTests
    {
        private readonly ICopy<TestClass> testCandidate;

        public CopyOperationRegisterCreateFromFactoryTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ITestFactory>().To<TestFactory>();

            this.testCandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var source = new TestClass { Value1 = 45 };
            var target = new TestClass();

            // Act
            this.testCandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value1.Should().Be(888);
            target.Value2.Should().Be(999);
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