using System;
using System.Collections.Generic;
using System.Text;
using BBT.StructureTools.Compare;
using BBT.StructureTools.Compare.Helper;
using BBT.StructureTools.Convert;
using BBT.StructureTools.Initialization;
using BBT.StructureTools.Tests.Compare.Intention;
using BBT.StructureTools.Tests.TestTools;
using FluentAssertions;
using Ninject;
using Xunit;

namespace BBT.StructureTools.Tests.Compare
{
    public class ToManyComparerTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> mComparer;

        public ToManyComparerTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICompareRegistrations<TestClass, ITestCompareIntention>>().To<TestClassCompareRegistrations>();
            kernel.Bind<ICompareRegistrations<TestClassListOfChildrenItem, ITestCompareIntention>>().To<TestClassListOfChildrenItemCompareRegistrations>();

            this.mComparer = kernel.Get<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_SameInstance_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                ListOfChildren =
                                         new List<TestClassListOfChildrenItem>
                                             {
                                                 new TestClassListOfChildrenItem(),
                                                 new TestClassListOfChildrenItem()
                                             }
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_SameInstanceButListNull_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                // Explicitly set null
                ListOfChildren = null
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_DifferentInstancesSameList_MustReturnTrue()
        {
            // Arrange
            var lListOfChildren =
                new List<TestClassListOfChildrenItem>
                    {
                        new TestClassListOfChildrenItem { TestValue1 = 1 },
                        new TestClassListOfChildrenItem { TestValue1 = 1 }
                    };

            var lTestClass = new TestClass()
            {
                // Explicitly set to same list instance
                ListOfChildren = lListOfChildren
            };

            var lTestClass2 = new TestClass()
            {
                // Explicitly set to same list instance
                ListOfChildren = lListOfChildren
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_DifferentInstancesDifferentListsSameValues_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 }
                                                          }
            };

            var lTestClass2 = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 }
                                                           }
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_DifferentInstancesDifferentListsSameValuesDifferentListPositions_MustReturnTrue()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 }
                                                          }
            };

            var lTestClass2 = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 }
                                                           }
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_DifferentInstancesDifferentListsDifferentValues_MustReturnFalse()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 }
                                                          }
            };

            var lTestClass2 = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 }
                                                           }
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeFalse();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Fact]
        public void Compare_DifferentInstancesDifferentListsDifferentValuesDifferentListCounts_MustReturnFalse()
        {
            // Arrange
            var lTestClass = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 }
                                                          }
            };

            var lTestClass2 = new TestClass()
            {
                // Explicitly set to different list instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 }
                                                           }
            };

            // Act
            var lResult = this.mComparer.Equals(lTestClass, lTestClass2);

            // Assert
            lResult.Should().BeFalse();
        }

        #region private test classes and test class helpers

        private class TestClass
        {
            public List<TestClassListOfChildrenItem> ListOfChildren { private get; set; }

            public List<TestClassListOfChildrenItem> GetChildren()
            {
                return this.ListOfChildren;
            }
        }

        private class TestClassListOfChildrenItem
        {
            public int TestValue1 { get; set; }

            public int TestValue2 { get; set; }
        }

        private class TestClassCompareRegistrations : ICompareRegistrations<TestClass, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterToManyRelationship(
                    aX => aX.GetChildren(),
                    IocHandler.Instance.IocResolver.GetInstance<IComparer<TestClassListOfChildrenItem, ITestCompareIntention>>());
            }
        }

        private class TestClassListOfChildrenItemCompareRegistrations : ICompareRegistrations<TestClassListOfChildrenItem, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassListOfChildrenItem> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterAttribute(aX => aX.TestValue1);
            }
        }
        #endregion
    }
}
