namespace BBT.StructureTools.Tests.Compare
{
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Exclusions;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    /// <summary>
    /// Test for Compare infrastructure with object attributes.
    /// </summary>
    public class ComparerWithObjectAttributeTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> testCandidate;

        public ComparerWithObjectAttributeTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<IComparerRegistrations<TestClass, ITestCompareIntention>>().To<TestClassComparerRegistrations>();

            this.testCandidate = kernel.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        [Fact]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute1 = new TestAttribute(),
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNull_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute1 = null,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqual_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute2 };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testClassA = new TestClass { TestAttribute2 = testAttribute };
            var testClassB = new TestClass { TestAttribute2 = testAttribute };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute2 };
            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  x => x.TestAttribute1),
                                          };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB, System.Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #region private test classes and test class helpers
        private class TestClass
        {
            public TestAttribute TestAttribute1 { get; set; }

            public TestAttribute TestAttribute2 { get; set; }
        }

        private class TestAttribute
        {
        }

        private class TestClassComparerRegistrations : IComparerRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterAttribute(x => x.TestAttribute1);
            }
        }
        #endregion
    }
}
