namespace BBT.StructureTools.Tests.Copy.PostProcessing
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationPostProcessingWithoutUsingParamsIntTest
    {
        #region setup and members

        private readonly ICopy<ITestClass> testcandidate;

        public CopyOperationPostProcessingWithoutUsingParamsIntTest()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<ITestClass>>().To<TestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<ITestClass>>();
        }

        #endregion

        #region Tests

        /// <summary>
        /// Tests for the execution of a single post processing which was registered via
        /// Copy Registrations.
        /// </summary>
        [Fact]
        public void MustExecuteRegisteredSinglePostProcessings()
        {
            // Arrange
            var testClassOriginal = new TestClass();
            var testClassCopy = new TestClass();

            // Act
            this.testcandidate.Copy(testClassOriginal, testClassCopy, new List<IBaseAdditionalProcessing>());

            // Assert
            testClassCopy.TestValue.Should().Be(234);
            testClassCopy.TestValue2.Should().Be(0);
        }

        #endregion
        #region Testdata

        /// <summary>
        /// Interface for a test class.
        /// </summary>
        private interface ITestClass
        {
            /// <summary>
            /// Gets or sets a Test value.
            /// </summary>
            int TestValue { get; set; }

            /// <summary>
            /// Gets or sets a Test value.
            /// </summary>
            int TestValue2 { get; set; }
        }

        /// <summary>
        /// Implementation of <see cref="ITestClass"/>.
        /// </summary>
        private class TestClass : ITestClass
        {
            /// <summary>
            /// Gets or sets a Test value.
            /// </summary>
            public int TestValue { get; set; }

            /// <summary>
            /// Gets or sets a Test value.
            /// </summary>
            public int TestValue2 { get; set; }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<ITestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<ITestClass> registrations)
            {
                registrations.Should().NotBeNull();
                registrations.RegisterPostProcessings(
                    new GenericCopyPostProcessing<ITestClass>((source, target) => target.TestValue = 234));
            }
        }
        #endregion
    }
}
