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
    public class ComparerWithValueAttributeTests
    {
        private readonly IComparer<TestClass, ITestCompareIntention> testcandidate;

        public ComparerWithValueAttributeTests(IIocContainer iocContainer)
        {
            iocContainer.Initialize();

            iocContainer.RegisterSingleton<ICompareRegistrations<TestClass, ITestCompareIntention>, TestClassCompareRegistrations>();

            this.testcandidate = iocContainer.GetInstance<IComparer<TestClass, ITestCompareIntention>>();
        }

        [Test]
        public void Equals_WhenSameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass { Value1 = 45 };

            // Act
            var result = this.testcandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeValuesEqual_MustReturnTrue()
        {
            // Arrange
            var testClassA = new TestClass { Value1 = 45 };
            var testClassB = new TestClass { Value1 = 45 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeValuesNotEqual_MustReturnFalse()
        {
            // Arrange
            var testClassA = new TestClass { Value1 = 45 };
            var testClassB = new TestClass { Value1 = 44 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Equals_WhenAttributeValuesNotEqualButNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testClassA = new TestClass { Value2 = 45 };
            var testClassB = new TestClass { Value2 = 44 };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Equals_WhenAttributeValuesNotEqualButExcluded_MustReturnTrue()
        {
            // Arrange
            var testClassA = new TestClass { Value1 = 45 };
            var testClassB = new TestClass { Value1 = 44 };
            var comparerExclusions = new List<IComparerExclusion> { new PropertyComparerExclusion<TestClass>(x => x.Value1) };

            // Act
            var result = this.testcandidate.Equals(testClassA, testClassB, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        private class TestClass
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterAttribute(x => x.Value1);
            }
        }
    }
}
