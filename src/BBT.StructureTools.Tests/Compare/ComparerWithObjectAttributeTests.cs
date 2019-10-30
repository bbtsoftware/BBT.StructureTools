using System.Collections.Generic;
using BBT.StructureTools.Compare;
using BBT.StructureTools.Compare.Exclusions;
using BBT.StructureTools.Compare.Helper;
using BBT.StructureTools.Tests.Compare.Intention;
using BBT.StructureTools.Tests.TestTools;
using FluentAssertions;
using Ninject;
using Xunit;

namespace BBT.StructureTools.Tests.Compare
{
    /// <summary>
    /// Test for Comparer infrastructure with object attributes.
    /// </summary>
    public class ComparerWithObjectAttributeTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithObjectAttributeTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();

            this.testcandidate = kernel.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        [Fact]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute1 = new TestAttribute()
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNull_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute1 = null
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute1 = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute1 = lTestAttribute };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqual_MustReturnFalse()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute1 = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute1 = lTestAttribute2 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute2 = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute2 = lTestAttribute };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute1 = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute1 = lTestAttribute2 };
            var lComparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  aX => aX.TestAttribute1)
                                          };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB, new IBaseAdditionalProcessing[0], lComparerExclusions);

            // Assert
            lResult.Should().BeTrue();
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

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterAttribute(aX => aX.TestAttribute1);
            }
        }
        #endregion
    }
}
