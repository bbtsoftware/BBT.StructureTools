// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy.CopyOperationPostProcessing
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    /// <summary>
    /// Integration tests for <see cref="ICopyOperationPostProcessing{T}"/>.
    /// </summary>
    public class CopyOperationPostProcessingUsingParamsIntTests
    {
        #region setup and members

        private ICopy<ITestClass> copy;

        public CopyOperationPostProcessingUsingParamsIntTests()
        {
            var container = TestIocContainer.Initialize();

            container.Bind<ICopyRegistrations<ITestClass>>().To<TestClassCopyRegistrations>();
            this.copy = container.Get<ICopy<ITestClass>>();
        }

        #endregion

        #region Tests

        /// <summary>
        /// Tests for the execution of a single post processing which was registered via
        /// Copy Registrations.
        /// </summary>
        [Fact]
        public void MustExecuteRegisteredPostProcessingsWhenParamsAreUsed()
        {
            // Arrange
            var testClassOriginal = new TestClass();
            var testClassCopy = new TestClass();

            // Act
            this.copy.Copy(testClassOriginal, testClassCopy, new List<IBaseAdditionalProcessing>());

            // Assert
            testClassCopy.TestValue.Should().Be(234);
            testClassCopy.TestValue2.Should().Be(4321);
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
                StructureToolsArgumentChecks.NotNull(registrations, nameof(registrations));
                registrations.RegisterPostProcessings(
                    new GenericCopyPostProcessing<ITestClass>((source, target) => target.TestValue = 234),
                    new GenericCopyPostProcessing<ITestClass>((source, target) => target.TestValue2 = 4321));
            }
        }
        #endregion
    }
}