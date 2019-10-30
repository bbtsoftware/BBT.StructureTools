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
using Ninject.Activation;
using Xunit;

namespace BBT.StructureTools.Tests.Compare
{
    public class SubComparerUsingComparerTests
    {
        #region Members, Setup
        private readonly IComparer<TestClassChild, ITestCompareIntention> testcandidate;
        private static IComparer<TestClassParent, ITestCompareIntention> ParentCompare;


        public SubComparerUsingComparerTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICompareRegistrations<TestClassChild, ITestCompareIntention>>().To<TestClassChildCompareRegistrations>();
            kernel.Bind<ICompareRegistrations<TestClassParent, ITestCompareIntention>>().To<TestClassParentCompareRegistrations>();

            ParentCompare = kernel.Get<IComparer<TestClassParent, ITestCompareIntention>>();
            this.testcandidate = kernel.Get<IComparer<TestClassChild, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_SameInstance_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClassChild();

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesSameAttributes_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClassChild
            {
                TestValue1 = 1
            };
            var lTestClass2 = new TestClassChild
            {
                TestValue1 = 1
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributes_MustReturnFalse()
        {
            // Arrange
            var lTestClass = new TestClassChild
            {
                TestValue1 = 1
            };
            var lTestClass2 = new TestClassChild
            {
                TestValue1 = 11
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeFalse();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesNotRegistered_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClassChild
            {
                TestValue2 = 1
            };
            var lTestClass2 = new TestClassChild
            {
                TestValue2 = 11
            };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesButExcludedByParent_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClassChild
            {
                TestValue1 = 1
            };
            var lTestClass2 = new TestClassChild
            {
                TestValue1 = 11
            };

            var lComparerExclusions = new List<IComparerExclusion>
                                          {
                                              new PropertyComparerExclusion<TestClassParent>(aX => aX.TestValue1)
                                          };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass2, new IBaseAdditionalProcessing[0], lComparerExclusions);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests equals.
        /// </summary>
        [Fact]
        public void Equals_DifferentInstancesNotSameAttributesButExcludedBySubCompareExclusion_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClassChild
            {
                TestValue1 = 1
            };
            var lTestClass2 = new TestClassChild
            {
                TestValue1 = 11
            };

            var lComparerExclusions = new List<IComparerExclusion>
                                          {
                                              new SubInterfaceComparerExclusion<TestClassParent>()
                                          };

            // Act
            var lResult = this.testcandidate.Equals(lTestClass, lTestClass2, new IBaseAdditionalProcessing[0], lComparerExclusions);

            // Assert
            lResult.Should().BeTrue();
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
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassParent> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterAttribute(aX => aX.TestValue1);
            }
        }

        private class TestClassChildCompareRegistrations : ICompareRegistrations<TestClassChild, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassChild> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterSubCompare(ParentCompare);
            }
        }
        #endregion
    }
}
