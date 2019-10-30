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
    public class ComparerWithObjectsAndValueAttributesTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithObjectsAndValueAttributesTests()
        {
            var kernel = TestIoContainer.Initialize();

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
                TestAttribute1 = new TestAttribute(),
                TestValue2 = 1
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenSameInstanceAndObjectAttributeNullButValueNot_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass
            {
                // Explicit null init on purpose
                TestAttribute1 = null,
                TestValue1 = 2
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValueEqual_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestClassA = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 1
            };
            var lTestClassB = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 1
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsButNotValueEqual_MustReturnFalse()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestClassA = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 1
            };
            var lTestClassB = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 2
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsNotButValueEqual_MustReturnFalse()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 1
            };
            var lTestClassB = new TestClass
            {
                TestAttribute1 = lTestAttribute2,
                TestValue1 = 1
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValuesNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass
            {
                TestAttribute2 = lTestAttribute,
                TestValue2 = 212
            };
            var lTestClassB = new TestClass
            {
                TestAttribute2 = lTestAttribute2,
                TestValue2 = 21
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeObjectsAndValuesNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var lTestAttribute = new TestAttribute();
            var lTestAttribute2 = new TestAttribute();
            var lTestClassA = new TestClass
            {
                TestAttribute1 = lTestAttribute,
                TestValue1 = 1
            };
            var lTestClassB = new TestClass
            {
                TestAttribute1 = lTestAttribute2,
                TestValue1 = 2
            };
            var lComparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(
                                                  aX => aX.TestAttribute1),
                                              new PropertyComparerExclusion<TestClass>(
                                                  aX => aX.TestValue1)
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

            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestAttribute
        {
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterAttribute(aX => aX.TestAttribute1)
                    .RegisterAttribute(aX => aX.TestValue1);
            }
        }
        #endregion
    }
}
