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

    public class SubComparerUsingComparerTests
    {
        #region Members, Setup
        private static IComparer<TestClassParent, ITestCompareIntention> parentCompare;
        private readonly IComparer<TestClassChild, ITestCompareIntention> testCandidate;

        public SubComparerUsingComparerTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClassChild, ITestCompareIntention>>().To<TestClassChildCompareRegistrations>();
            kernel.Bind<ICompareRegistrations<TestClassParent, ITestCompareIntention>>().To<TestClassParentCompareRegistrations>();

            parentCompare = kernel.Get<IComparer<TestClassParent, ITestCompareIntention>>();
            this.testCandidate = kernel.Get<IComparer<TestClassChild, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_SameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClassChild();

            // Act
            var result = this.testCandidate.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesSameAttributes_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClassChild
            {
                TestValue1 = 1,
            };
            var testClass2 = new TestClassChild
            {
                TestValue1 = 1,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass2);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributes_MustReturnFalse()
        {
            // Arrange
            var testClass = new TestClassChild
            {
                TestValue1 = 1,
            };
            var testClass2 = new TestClassChild
            {
                TestValue1 = 11,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass2);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesNotRegistered_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClassChild
            {
                TestValue2 = 1,
            };
            var testClass2 = new TestClassChild
            {
                TestValue2 = 11,
            };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass2);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesButExcludedByParent_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClassChild
            {
                TestValue1 = 1,
            };
            var testClass2 = new TestClassChild
            {
                TestValue1 = 11,
            };

            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClassParent>(x => x.TestValue1),
                                          };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass2, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesButExcludedBySubCompareExclusion_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClassChild
            {
                TestValue1 = 1,
            };
            var testClass2 = new TestClassChild
            {
                TestValue1 = 11,
            };

            var comparerExclusions = new List<IComparerExclusion>
                                          {
                                              new SubInterfaceComparerExclusion<TestClassParent>(),
                                          };

            // Act
            var result = this.testCandidate.Equals(testClass, testClass2, Array.Empty<IBaseAdditionalProcessing>(), comparerExclusions);

            // Assert
            result.Should().BeTrue();
        }

        #region private test classes and test class helpers

        private class TestClassParent
        {
            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestClassChild : TestClassParent
        {
        }

        private class TestClassParentCompareRegistrations : ICompareRegistrations<TestClassParent, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassParent> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterAttribute(x => x.TestValue1);
            }
        }

        private class TestClassChildCompareRegistrations : ICompareRegistrations<TestClassChild, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassChild> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterSubCompare(parentCompare);
            }
        }
        #endregion
    }
}
