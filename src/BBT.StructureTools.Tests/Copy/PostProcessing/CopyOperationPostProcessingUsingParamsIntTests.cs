using System.Collections.Generic;
using BBT.StructureTools.Copy;
using BBT.StructureTools.Copy.Helper;
using BBT.StructureTools.Copy.Processing;
using BBT.StructureTools.Tests.TestTools;
using BBT.StructureTools.Tests.TestTools.IoC;
using FluentAssertions;
using Unity;
using Xunit;

namespace BBT.StructureTools.Tests.Copy.PostProcessing
{
    /// <summary>
    /// Integration tests for <see cref="ICopyOperationPostProcessing{T}"/>.
    /// </summary>
    public class CopyOperationPostProcessingUsingParamsIntTests : BaseIocFixture
    {
        #region setup and members

        private readonly ICopy<ITestClass> testcandidate;

        public CopyOperationPostProcessingUsingParamsIntTests(IocFixtureData fixtureData)
            : base(fixtureData)
        {
            this.FixtureData.IocContainer.RegisterType<ICopyRegistrations<ITestClass>, TestClassCopyRegistrations>();

            this.testcandidate = this.FixtureData.IocContainer.Resolve<ICopy<ITestClass>>();
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
            var lTestClassOriginal = new TestClass();
            var lTestClassCopy = new TestClass();

            // Act
            this.testcandidate.Copy(lTestClassOriginal, lTestClassCopy, new List<IBaseAdditionalProcessing>());

            // Assert
            lTestClassCopy.TestValue.Should().Be(234);
            lTestClassCopy.TestValue2.Should().Be(4321);
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
            public void DoRegistrations(ICopyHelperRegistration<ITestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterPostProcessings(
                    new GenericCopyPostProcessing<ITestClass>((aSource, aTarget) => aTarget.TestValue = 234),
                    new GenericCopyPostProcessing<ITestClass>((aSource, aTarget) => aTarget.TestValue2 = 4321));
            }
        }
        #endregion
    }
}
