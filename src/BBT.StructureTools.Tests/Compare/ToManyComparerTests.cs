namespace BBT.StructureTools.Tests.Compare
{
    using System.Collections.Generic;
    using BBT.StructureTools.Compare;
    using BBT.StructureTools.Compare.Helper;
    using BBT.StructureTools.Initialization;
    using BBT.StructureTools.Tests.Compare.Intention;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ToManyComparerTests
    {
        #region Members, Setup
        private readonly IComparer<TestClass, ITestCompareIntention> comparer;

        public ToManyComparerTests()
        {
            var iocContainer = new NinjectIocContainer();

            iocContainer.RegisterSingleton<ICompareRegistrations<TestClass, ITestCompareIntention>, TestClassCompareRegistrations>();
            iocContainer.RegisterSingleton<ICompareRegistrations<TestClassListOfChildrenItem, ITestCompareIntention>, TestClassListOfChildrenItemCompareRegistrations>();

            this.comparer = iocContainer.GetInstance<IComparer<TestClass, ITestCompareIntention>>();
        }

        #endregion

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_SameInstance_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass()
            {
                ListOfChildren =
                                         new List<TestClassListOfChildrenItem>
                                             {
                                                 new TestClassListOfChildrenItem(),
                                                 new TestClassListOfChildrenItem(),
                                             },
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_SameInstanceButistNull_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass()
            {
                // Explicitly set null
                ListOfChildren = null,
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_DifferentInstancesSameist_MustReturnTrue()
        {
            // Arrange
            var listOfChildren =
                new List<TestClassListOfChildrenItem>
                    {
                        new TestClassListOfChildrenItem { TestValue1 = 1 },
                        new TestClassListOfChildrenItem { TestValue1 = 1 },
                    };

            var testClass = new TestClass()
            {
                // Explicitly set to same ist instance
                ListOfChildren = listOfChildren,
            };

            var testClass2 = new TestClass()
            {
                // Explicitly set to same ist instance
                ListOfChildren = listOfChildren,
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass2);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_DifferentInstancesDifferentistsSameValues_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                          },
            };

            var testClass2 = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                           },
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass2);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_DifferentInstancesDifferentistsSameValuesDifferentistPositions_MustReturnTrue()
        {
            // Arrange
            var testClass = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                          },
            };

            var testClass2 = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                           },
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass2);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_DifferentInstancesDifferentistsDifferentValues_MustReturnFalse()
        {
            // Arrange
            var testClass = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 1 },
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                          },
            };

            var testClass2 = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                           },
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass2);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests the compare infrastructure.
        /// </summary>
        [Test]
        public void Compare_DifferentInstancesDifferentistsDifferentValuesDifferentistCounts_MustReturnFalse()
        {
            // Arrange
            var testClass = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                          {
                                                              new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                          },
            };

            var testClass2 = new TestClass()
            {
                // Explicitly set to different ist instance
                ListOfChildren = new List<TestClassListOfChildrenItem>
                                                           {
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                               new TestClassListOfChildrenItem { TestValue1 = 2 },
                                                           },
            };

            // Act
            var result = this.comparer.Equals(testClass, testClass2);

            // Assert
            result.Should().BeFalse();
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
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterToManyRelationship(
                    x => x.GetChildren(),
                    IocHandler.Instance.IocResolver.GetInstance<IComparer<TestClassListOfChildrenItem, ITestCompareIntention>>());
            }
        }

        private class TestClassListOfChildrenItemCompareRegistrations : ICompareRegistrations<TestClassListOfChildrenItem, ITestCompareIntention>
        {
            public void DoRegistrations(IEqualityComparerHelperRegistration<TestClassListOfChildrenItem> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterAttribute(x => x.TestValue1);
            }
        }
        #endregion
    }
}
