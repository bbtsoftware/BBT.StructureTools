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
    public class ComparerWithValueAttributeTests
    {
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithValueAttributeTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();

            this.testcandidate = kernel.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        [Fact]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass { Value1 = 45 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeValuesEqual_MustReturnTrue()
        {
            // Arrange
            var lTestClassA = new TestClass { Value1 = 45 };
            var lTestClassB = new TestClass { Value1 = 45 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeValuesNotEqual_MustReturnFalse()
        {
            // Arrange
            var lTestClassA = new TestClass { Value1 = 45 };
            var lTestClassB = new TestClass { Value1 = 44 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeFalse();
        }

        [Fact]
        public void Equals_WhenAttributeValuesNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var lTestClassA = new TestClass { Value2 = 45 };
            var lTestClassB = new TestClass { Value2 = 44 };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB);

            // Assert
            lResult.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenAttributeValuesNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var lTestClassA = new TestClass { Value1 = 45 };
            var lTestClassB = new TestClass { Value1 = 44 };
            var lComparerExclusions = new List<IComparerExclusion> { new PropertyComparerExclusion<TestClass>(aX => aX.Value1) };

            // Act
            var lResult = this.testcandidate.Equals(lTestClassA, lTestClassB, new IBaseAdditionalProcessing[0], lComparerExclusions);

            // Assert
            lResult.Should().BeTrue();
        }

        private class TestClass
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterAttribute(aX => aX.Value1);
            }
        }
    }
}
