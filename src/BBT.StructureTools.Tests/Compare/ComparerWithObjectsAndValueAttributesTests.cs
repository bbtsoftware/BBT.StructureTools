namespace BBT.StructureTools.Tests.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Exclusions;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class ComparerWithObjectsAndValueAttributesTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> testCandidate;

        public ComparerWithObjectsAndValueAttributesTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();

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
                TestValue2 = 1,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNullButValueNot_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute1 = null,
                TestValue1 = 2,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValueEqual_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testClassA = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 1,
            };
            var testClassB = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 1,
            };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsButNotValueEqual_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testClassA = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 1,
            };
            var testClassB = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 2,
            };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotButValueEqual_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 1,
            };
            var testClassB = new TestClass
            {
                TestAttribute1 = testAttribute2,
                TestValue1 = 1,
            };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValuesNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass
            {
                TestAttribute2 = testAttribute,
                TestValue2 = 212,
            };
            var testClassB = new TestClass
            {
                TestAttribute2 = testAttribute2,
                TestValue2 = 21,
            };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValuesNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass
            {
                TestAttribute1 = testAttribute,
                TestValue1 = 1,
            };
            var testClassB = new TestClass
            {
                TestAttribute1 = testAttribute2,
                TestValue1 = 2,
            };
            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  x => x.TestAttribute1),
                                              new PropertyComparerExclusion<TestClass>(
                                                  x => x.TestValue1),
                                          };

            // Act
            var result = this.testCandidate.Equals(testClassA, testClassB, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #region private test classes and test class helpers
        private class TestClass
        {
            public TestAttribute TestAttribute1 { get; set; }

            public TestAttribute TestAttribute2 { get; set; }

            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestAttribute
        {
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterAttribute(x => x.TestAttribute1)
                    .RegisterAttribute(x => x.TestValue1);
            }
        }
        #endregion
    }
}
