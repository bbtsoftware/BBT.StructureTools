using System;
using System.Collections.Generic;
using System.Text;
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
    public class ComparerWithObjectAttributeWithDistinguishedComparerIntTests
    {
        #region Members, Setup

        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;
        private static IComparer<TestAttribute, ITestCompareIntention> DistinguishedComparer;

        public ComparerWithObjectAttributeWithDistinguishedComparerIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();
            kernel.Bind<ICompareRegistrations<TestAttribute, ITestCompareIntention>>().To<TestAttributeCompareRegistrations>();

            DistinguishedComparer = kernel.Get<IComparer<TestAttribute, ITestCompareIntention>>();
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
            var lTestClass = new TestClass
            {
                // Explicit instance init on purpose
                TestAttribute = new TestAttribute()
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNull_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute = null
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsEqual_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute = lTestAttribute };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualButHaveSameValue_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute = lTestAttribute2 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenBaseModelAttributeObjectsNotEqualAndHaveDifferentValue_MustReturnFalse()
        {
            // Arrange
            var lTestAttribute = new TestAttribute { TestValue1 = 55 };
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass { TestAttribute = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute = lTestAttribute2 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeFalse();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute() { TestValue2 = 2 };
            var lTestAttribute2 = new TestAttribute() { TestValue2 = 1 };
            var lTestClassA = new TestClass { TestAttribute = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute = lTestAttribute2 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests IComparer.Equals.
        /// </summary>
        [Fact]
        public void Equals_WhenAttributeObjectsNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute() { TestValue1 = 2 };
            var lTestAttribute2 = new TestAttribute() { TestValue1 = 1 };
            var lTestClassA = new TestClass { TestAttribute = lTestAttribute };
            var lTestClassB = new TestClass { TestAttribute = lTestAttribute2 };
            var lComparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  aX => aX.TestAttribute)
                                          };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB, new IBaseAdditionalProcessing[0], lComparerExclusions);

            // Assert
            lResult.Should().BeTrue();
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
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestAttribute> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterAttribute(aX => aX.TestValue1);
            }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterAttributeWithDistinguishedComparer(
                    aX => aX.TestAttribute,
                    ComparerWithObjectAttributeWithDistinguishedComparerIntTests.DistinguishedComparer);
            }
        }

        #endregion
    }
}
