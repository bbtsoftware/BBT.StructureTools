namespace BBT.StructureTools.Tests.Compare
{
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Exclusions;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using NUnit.Framework;

    /// <summary>
    /// Test for Comparer infrastructure with object attributes.
    /// </summary>
    [TestFixture]
    public class ComparerWithObjectAttributeTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithObjectAttributeTests()
        {
            var kernel = new NinjectIocContainer();

            kernel.RegisterSingleton<ICompareRegistrations<TestClass, ITestCompareIntention>, TestClassCompareRegistrations>();

            this.testcandidate = kernel.GetInstance<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        [Test]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute1 = new TestObjProperty(),
            };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenSameInstanceAndObjectAttributeNull_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute1 = null,
            };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestObjProperty();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeObjectsNotEqual_MustReturnFalse()
        {
            // Arrange
            var testAttribute = new TestObjProperty();
            var testAttribute2 = new TestObjProperty();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute2 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestObjProperty();
            var testClassA = new TestClass { TestAttribute2 = testAttribute };
            var testClassB = new TestClass { TestAttribute2 = testAttribute };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testAttribute = new TestObjProperty();
            var testAttribute2 = new TestObjProperty();
            var testClassA = new TestClass { TestAttribute1 = testAttribute };
            var testClassB = new TestClass { TestAttribute1 = testAttribute2 };
            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  x => x.TestAttribute1),
                                          };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB, System.Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #region private test classes and test class helpers
        private class TestClass
        {
            public TestObjProperty TestAttribute1 { get; set; }

            public TestObjProperty TestAttribute2 { get; set; }
        }

        private class TestObjProperty
        {
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
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
