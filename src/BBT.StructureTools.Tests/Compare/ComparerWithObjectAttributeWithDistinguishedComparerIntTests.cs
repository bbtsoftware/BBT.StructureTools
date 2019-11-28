namespace BBT.StructureTools.Tests.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Exclusions;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools.IoC;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [TestFixtureSource(typeof(IocTestFixtureSource), "IocContainers")]
    public class ComparerWithObjectAttributeWithDistinguishedComparerIntTests
    {
        #region Members, Setup

        private static IComparer<TestWithProperties, ITestCompareIntention> distinguishedComparer;
        private readonly IComparer<TestClass, ITestCompareIntention> testCandidate;

        public ComparerWithObjectAttributeWithDistinguishedComparerIntTests(IIocContainer iocContainer)
        {
            iocContainer.Initialize();

            iocContainer.RegisterSingleton<ICompareRegistrations<TestClass, ITestCompareIntention>, TestClassCompareRegistrations>();
            iocContainer.RegisterSingleton<ICompareRegistrations<TestWithProperties, ITestCompareIntention>, TestAttributeCompareRegistrations>();

            distinguishedComparer = iocContainer.GetInstance<IComparer<TestWithProperties, ITestCompareIntention>>();
            this.testCandidate = iocContainer.GetInstance<IComparer<TestClass, ITestCompareIntention>>();
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
            var result = this.testCandidate.Equals(testClass, testClass);

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
            var result = this.testCandidate.Equals(testClass, testClass);

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
            var result = this.testCandidate.Equals(testClassA, testClassB);

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
            var result = this.testCandidate.Equals(testClassA, testClassB);

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
            var result = this.testCandidate.Equals(testClassA, testClassB);

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
            var result = this.testCandidate.Equals(testClassA, testClassB);

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
            var result = this.testCandidate.Equals(testClassA, testClassB, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

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
