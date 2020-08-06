namespace BBT.StructureTools.Tests.Compare
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Exclusions;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    /// <summary>
    /// Test for Comparer infrastructure with object attributes.
    /// </summary>
    /// <remarks>
    /// Raison d'Être for this integration test collection is that before a refactoring,
    /// <see cref="IEqualityComparerHelperRegistration{T}.RegisterAttributeWithDistinguishedComparer{TComparer,TIntention}"/>
    /// only accepted classes inheriting <see cref="IBaseModel"/> as parameter. The tests here are
    /// the same as defined in <see cref="ComparerWithObjectAttributeWithDistinguishedComparerIntTests"/>
    /// but explicitly test using an attribute in the <see cref="TestClass"/> which is inheriting
    /// from <see cref="IBaseModel"/> and <see cref="BaseModel"/> (called
    /// <see cref="BaseModelTestAttribute"/>).
    /// It's paranoid yet justified since we can't be sure about no
    /// black magic going on within <see cref="BaseModel"/>, and this way we can at least give our
    /// best to ensure that we're somewhat backwards compatible an don't break anything important
    /// by accident.
    /// </remarks>
    public class ComparerWithObjectInheritingFromBaseModelAttributeWithDistinguishedComparerTests
    {
        #region Members, Setup, Teardown
        private static IKernel container;
        private IComparer<TestClass, ITestCompareIntention> comparer;

        public ComparerWithObjectInheritingFromBaseModelAttributeWithDistinguishedComparerTests()
        {
            container = TestIocContainer.Initialize();
            container.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();
            container.Bind<ICompareRegistrations<BaseModelTestAttribute, ITestCompareIntention>>().To<BaseModelTestAttributeCompareRegistrations>();

            this.comparer = container.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        #region Test implementations

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
                BaseModelTestAttribute = new BaseModelTestAttribute(),
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass);

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
                BaseModelTestAttribute = null,
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass);

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
            var baseModelTestAttribute = new BaseModelTestAttribute();
            var testClassA = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };
            var testClassB = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };

            // Act
            var result = this.comparer.Equals(testClassA, testClassB);

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
            var baseModelTestAttribute = new BaseModelTestAttribute();
            var baseModelTestAttribute2 = new BaseModelTestAttribute();
            var testClassA = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };
            var testClassB = new TestClass { BaseModelTestAttribute = baseModelTestAttribute2 };

            // Act
            var result = this.comparer.Equals(testClassA, testClassB);

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
            var baseModelTestAttribute = new BaseModelTestAttribute { TestValue1 = 55 };
            var baseModelTestAttribute2 = new BaseModelTestAttribute();
            var testClassA = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };
            var testClassB = new TestClass { BaseModelTestAttribute = baseModelTestAttribute2 };

            // Act
            var result = this.comparer.Equals(testClassA, testClassB);

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
            var baseModelTestAttribute = new BaseModelTestAttribute() { TestValue2 = 2 };
            var baseModelTestAttribute2 = new BaseModelTestAttribute() { TestValue2 = 1 };
            var testClassA = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };
            var testClassB = new TestClass { BaseModelTestAttribute = baseModelTestAttribute2 };

            // Act
            var result = this.comparer.Equals(testClassA, testClassB);

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
            var baseModelTestAttribute = new BaseModelTestAttribute() { TestValue1 = 2 };
            var baseModelTestAttribute2 = new BaseModelTestAttribute() { TestValue1 = 1 };
            var testClassA = new TestClass { BaseModelTestAttribute = baseModelTestAttribute };
            var testClassB = new TestClass { BaseModelTestAttribute = baseModelTestAttribute2 };
            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClass>(x => x.BaseModelTestAttribute),
                                          };

            // Act
            var result = this.comparer.Equals(testClassA, testClassB, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region private test classes and test class helpers
        private class TestClass
        {
            public BaseModelTestAttribute BaseModelTestAttribute { get; set; }
        }

        private class BaseModelTestAttribute
        {
            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "Class instantiated through IOC when IComparer<> is instantiated")]
        private class BaseModelTestAttributeCompareRegistrations : ICompareRegistrations<BaseModelTestAttribute, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<BaseModelTestAttribute> registrations)
            {
                StructureToolsArgumentChecks.NotNull(registrations, nameof(registrations));

                registrations.RegisterAttribute(x => x.TestValue1);
            }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                StructureToolsArgumentChecks.NotNull(registrations, nameof(registrations));

                registrations.RegisterAttributeWithDistinguishedComparer(
                    x => x.BaseModelTestAttribute,
                    container.Get<IComparer<BaseModelTestAttribute, ITestCompareIntention>>());
            }
        }
        #endregion
    }
}