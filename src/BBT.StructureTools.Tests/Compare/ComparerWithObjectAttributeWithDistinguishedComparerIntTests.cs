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

    public class ComparerWithObjectAttributeWithDistinguishedComparerIntTests
    {
        #region Members, Setup

        private static IComparer<TestAttribute, ITestCompareIntention> distinguishedComparer;
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithObjectAttributeWithDistinguishedComparerIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();
            kernel.Bind<ICompareRegistrations<TestAttribute, ITestCompareIntention>>().To<TestAttributeCompareRegistrations>();

            distinguishedComparer = kernel.Get<IComparer<TestAttribute, ITestCompareIntention>>();
            this.testcandidate = kernel.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute = new TestAttribute(),
            };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNull_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute = null,
            };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testClassA = new TestClass { TestAttribute = testAttribute };
            var testClassB = new TestClass { TestAttribute = testAttribute };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualButHaveSameValue_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute();
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass { TestAttribute = testAttribute };
            var testClassB = new TestClass { TestAttribute = testAttribute2 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualAndHaveDifferentValue_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestAttribute { TestValue1 = 55 };
            var testAttribute2 = new TestAttribute();
            var testClassA = new TestClass { TestAttribute = testAttribute };
            var testClassB = new TestClass { TestAttribute = testAttribute2 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute() { TestValue2 = 2 };
            var testAttribute2 = new TestAttribute() { TestValue2 = 1 };
            var testClassA = new TestClass { TestAttribute = testAttribute };
            var testClassB = new TestClass { TestAttribute = testAttribute2 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestAttribute() { TestValue1 = 2 };
            var testAttribute2 = new TestAttribute() { TestValue1 = 1 };
            var testClassA = new TestClass { TestAttribute = testAttribute };
            var testClassB = new TestClass { TestAttribute = testAttribute2 };
            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  x => x.TestAttribute),
                                          };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #region private test classes and test class helpers

        private class TestClass
        {
            public TestAttribute TestAttribute { get; set; }
        }

        private class TestAttribute
        {
            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestAttributeCompareRegistrations : ICompareRegistrations<TestAttribute, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestAttribute> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterAttribute(x => x.TestValue1);
            }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterAttributeWithDistinguishedComparer(
                    x => x.TestAttribute,
                    ComparerWithObjectAttributeWithDistinguishedComparerIntTests.distinguishedComparer);
            }
        }

        #endregion
    }
}
