namespace BBT.StructureTools.Tests.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StrategyPattern;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Strategy;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CopyOperationCreateToManyFromGenericStrategyIntTests
    {
        #region setup

        private readonly ICopy<ParentTestClass> testcandidate;

        public CopyOperationCreateToManyFromGenericStrategyIntTests()
        {
            var kernel = new NinjectIocContainer();

            kernel.RegisterSingleton<ICopyRegistrations<IParentTestClass>, TestClassCopyRegistrations>();
            kernel.RegisterSingleton<ICopyRegistrations<IChildTestClass>, ChildTestClassCopyRegistrations>();
            kernel.RegisterSingleton<IGenericStrategyProvider<TestStrategy, IChildTestClass>, TestFactory>();
            kernel.RegisterSingleton<ITestStrategy, TestStrategy>();

            this.testcandidate = kernel.GetInstance<ICopy<IParentTestClass>>();
        }

        #endregion

        /// <summary>
        /// Tests whether the strategy is actually being used during copy.
        /// </summary>
        [Test]
        public void MustUseStrategyWhenCopying()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 1 });
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 2 });
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 3 });

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            var testClassParentCopyChildren = testClassParentCopy.Children.Cast<ChildTestClass>().ToList();
            var testClassParentOriginchildren = testClassParentOriginal.Children.Cast<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            testClassParentCopyChildren.Count.Should().Be(3);

            testClassParentCopyChildren.ElementAt(0).TestValue.Should().Be(testClassParentOriginchildren.ElementAt(0).TestValue * 2);
            testClassParentCopyChildren.ElementAt(1).TestValue.Should().Be(testClassParentOriginchildren.ElementAt(1).TestValue * 2);
            testClassParentCopyChildren.ElementAt(2).TestValue.Should().Be(testClassParentOriginchildren.ElementAt(2).TestValue * 2);

            foreach (var copiedChild in testClassParentCopyChildren)
            {
                foreach (var originchild in testClassParentOriginal.Children)
                {
                    originchild.Should().NotBeSameAs(copiedChild);
                }
            }
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_MustFailWhenChildrenistNull()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.MakeChildrenCollectionNull();

            var testClassParentCopy = new ParentTestClass();

            // Act / Assert throws
            Assert.Throws<ArgumentNullException>(() =>
                this.testcandidate.Copy(
                    testClassParentOriginal,
                    testClassParentCopy,
                    new List<IBaseAdditionalProcessing>()));
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_MustCopyEmptyCollection()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            // Also make sure the parent test class children collection, which is empty, was copied.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);
            testClassParentCopy.Children.Should().NotBeSameAs(testClassParentOriginal.Children);
            testClassParentCopy.Children.Should().NotBeNull();
        }

        #region test data

        /// <summary>
        /// An interface, defining a test class to be used as child in the tests above.
        /// </summary>
        private interface IChildTestClass
        {
            /// <summary>
            /// Gets or sets a value which can be used for testing purposes.
            /// </summary>
            int TestValue { get; set; }
        }

        /// <summary>
        /// Interface for parent test class.
        /// </summary>
        private interface IParentTestClass
        {
            /// <summary>
            /// Gets or sets the children.
            /// </summary>
            IList<IChildTestClass> Children { get; set; }

            /// <summary>
            /// Gets Identifier (use for debug).
            /// </summary>
            Guid Identifier { get; }

            /// <summary>
            /// Adds a child.
            /// </summary>
            /// <param name="child">child item.</param>
            void AddChild(ChildTestClass child);

            /// <summary>
            /// return the children.
            /// </summary>
            /// <returns>children collection.</returns>
            IList<IChildTestClass> GetChildren();

            /// <summary>
            /// Set's the internal children collection to null.
            /// </summary>
            void MakeChildrenCollectionNull();
        }

        private class ParentTestClass : IParentTestClass
        {
            public IList<IChildTestClass> Children { get; set; } = new List<IChildTestClass>();

            // ReSharper disable once ConvertToAutoProperty it's as intended here.
            // This identifier shall only be used to divide original and copied parent objects when
            // debugging the unit tests.
            public Guid Identifier { get; } = Guid.NewGuid();

            public void AddChild(ChildTestClass child)
            {
                this.Children.Add(child);
            }

            public IList<IChildTestClass> GetChildren()
            {
                return this.Children;
            }

            public void MakeChildrenCollectionNull()
            {
                this.Children = null;
            }
        }

        private class ChildTestClass : IChildTestClass
        {
            /// <summary>
            /// Gets or sets a value which can be used for testing purposes.
            /// </summary>
            public int TestValue { get; set; }
        }

        private class ChildTestClassCopyRegistrations : ICopyRegistrations<IChildTestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<IParentTestClass>
        {
            public TestClassCopyRegistrations()
            {
            }

            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> registrations)
            {
                registrations.Should().NotBeNull();
                registrations.RegisterCreateToManyFromGenericStrategy<ITestStrategy, IChildTestClass>(
                    x => x.Children.Cast<IChildTestClass>(),
                    x => x.Children,
                    x => x.CreateTestClass());
            }
        }

        /// <summary>
        /// Test interface for a strategy.
        /// </summary>
        private interface ITestStrategy : ICopyStrategy<IChildTestClass>
        {
            /// <summary>
            /// returns a new instance of <see cref="IChildTestClass"/>.
            /// </summary>
            IChildTestClass CreateTestClass();
        }

        private class TestStrategy : ITestStrategy
        {
            public bool IsResponsible(IChildTestClass criterion)
            {
                // For Test purposes.
                return true;
            }

            public IChildTestClass CreateTestClass() => new ChildTestClass();

            public IChildTestClass Create()
            {
                return new ChildTestClass();
            }

            public void Copy(IChildTestClass source, IChildTestClass target, ICopyCallContext copyCalcontext)
            {
                source.Should().NotBeNull();
                target.Should().NotBeNull();
                copyCalcontext.Should().NotBeNull();

                target.TestValue = source.TestValue * 2;
            }
        }

        private class TestFactory : IGenericStrategyProvider<TestStrategy, IChildTestClass>
        {
            public IEnumerable<TestStrategy> GetAllStrategies()
            {
                // Not needed for test scenario
                throw new NotImplementedException();
            }

            public TestStrategy GetStrategy(IChildTestClass criterion)
            {
                return new TestStrategy();
            }
        }
        #endregion
    }
}