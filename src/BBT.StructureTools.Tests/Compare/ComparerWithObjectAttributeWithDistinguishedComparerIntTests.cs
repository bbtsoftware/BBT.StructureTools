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
    using NUnit.Framework;

    [TestFixture]
    public class ComparerWithObjectAttributeWithDistinguishedComparerIntTests
    {
        #region Members, Setup

        private static IComparer<TestWithProperties, ITestCompareIntention> distinguishedComparer;
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithObjectAttributeWithDistinguishedComparerIntTests()
        {
            var kernel = new NinjectIocContainer();

            kernel.RegisterSingleton<ICompareRegistrations<TestClass, ITestCompareIntention>, TestClassCompareRegistrations>();
            kernel.RegisterSingleton<ICompareRegistrations<TestWithProperties, ITestCompareIntention>, TestAttributeCompareRegistrations>();

            distinguishedComparer = kernel.GetInstance<IComparer<TestWithProperties, ITestCompareIntention>>();
            this.testcandidate = kernel.GetInstance<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Test]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute = new TestWithProperties(),
            };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Test]
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
        [Test]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestWithProperties();
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
        [Test]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualButHaveSameValue_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestWithProperties();
            var testAttribute2 = new TestWithProperties();
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
        [Test]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualAndHaveDifferentValue_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestWithProperties { TestValue1 = 55 };
            var testAttribute2 = new TestWithProperties();
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
        [Test]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestWithProperties() { TestValue2 = 2 };
            var testAttribute2 = new TestWithProperties() { TestValue2 = 1 };
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
        [Test]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestWithProperties() { TestValue1 = 2 };
            var testAttribute2 = new TestWithProperties() { TestValue1 = 1 };
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
            public TestWithProperties TestAttribute { get; set; }
        }

        private class TestWithProperties
        {
            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestAttributeCompareRegistrations : ICompareRegistrations<TestWithProperties, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestWithProperties> registrations)
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
