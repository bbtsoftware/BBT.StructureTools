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
    using Ninject;
    using Xunit;

    public class CopyOperationCreateToManyFromGenericStrategyWithReverseRelationIntTests
    {
        #region setup

        private readonly ICopier<IParentTestClass> testCandidate;

        public CopyOperationCreateToManyFromGenericStrategyWithReverseRelationIntTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<IGenericStrategyProvider<TestStrategy, IChildTestClass>>().To<TestFactory>();
            kernel.Bind<ITestStrategy>().To<TestStrategy>();
            kernel.Bind<ICopierRegistrations<IParentTestClass>>().To<TestClassCopierRegistrations>();
            kernel.Bind<ICopierRegistrations<IChildTestClass>>().To<ChildTestClassCopierRegistrations>();

            this.testCandidate = kernel.Get<ICopier<IParentTestClass>>();
        }

        #endregion

        /// <summary>
        /// Tests whether the strategy is actually being used during copy.
        /// </summary>
        [Fact]
        public void MustUseStrategyWhenCopying()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 1, Parent = testClassParentOriginal });
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 2, Parent = testClassParentOriginal });
            testClassParentOriginal.AddChild(new ChildTestClass() { TestValue = 3, Parent = testClassParentOriginal });

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testCandidate.Copy(
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
                foreach (var originalChild in testClassParentOriginal.Children)
                {
                    originalChild.Should().NotBeSameAs(copiedChild);
                }
            }

            // Check for the correct referential reverse relation copy.
            foreach (var child in testClassParentCopyChildren)
            {
                child.Parent.Should().BeSameAs(testClassParentCopy);
            }
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustFailWhenChildrenistNull()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.MakeChildrenCollectionNull();

            var testClassParentCopy = new ParentTestClass();

            // Act / Assert throws
            Assert.Throws<ArgumentNullException>(() =>
                this.testCandidate.Copy(
                    testClassParentOriginal,
                    testClassParentCopy,
                    new List<IBaseAdditionalProcessing>()));
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustCopyEmptyCollection()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testCandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            // Also make sure the parent test class children collection, which is empty, was copied.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);
            testClassParentCopy.Children.Should().NotBeSameAs(testClassParentOriginal.Children);
            testClassParentCopy.Should().NotBeNull();
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

            /// <summary>
            /// Gets or sets reverse relation to the child's parent object.
            /// </summary>
            IParentTestClass Parent { get; set; }
        }

        /// <summary>
        /// Interface for parent test class.
        /// </summary>
        private interface IParentTestClass
        {
            /// <summary>
            /// Gets or sets the children.
            /// </summary>
            ICollection<IChildTestClass> Children { get; set; }

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
            ICollection<IChildTestClass> GetChildren();

            /// <summary>
            /// Set's the internal children collection to null.
            /// </summary>
            void MakeChildrenCollectionNull();
        }

        private class ParentTestClass : IParentTestClass
        {
            public ICollection<IChildTestClass> Children { get; set; } = new List<IChildTestClass>();

            public Guid Identifier { get; } = Guid.NewGuid();

            public void AddChild(ChildTestClass child)
            {
                this.Children.Add(child);
            }

            public ICollection<IChildTestClass> GetChildren()
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

            public IParentTestClass Parent { get; set; }
        }

        private class ChildTestClassCopierRegistrations : ICopierRegistrations<IChildTestClass>
        {
            // Further notice: This class is needed and it's registration via IoC container
            // is mandatory. Otherwise copying the child elements wouldn't work!
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }

        private class TestClassCopierRegistrations : ICopierRegistrations<IParentTestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterCreateToManyFromGenericStrategyWithReverseRelation<ITestStrategy, IChildTestClass>(
                    x => x.Children.Cast<IChildTestClass>(),
                    x => x.Children,
                    x => x.Parent);
            }
        }

        /// <summary>
        /// Test interface for a strategy.
        /// </summary>
        private interface ITestStrategy : ICopyStrategy<IChildTestClass>
        {
        }

        private class TestStrategy : ITestStrategy
        {
            public bool IsResponsible(IChildTestClass criterion)
            {
                // For Test purposes.
                return true;
            }

            public void Copy(IChildTestClass source, IChildTestClass target, ICopyCallContext copyCallContext)
            {
                source.Should().NotBeNull();
                target.Should().NotBeNull();
                copyCallContext.Should().NotBeNull();

                target.TestValue = source.TestValue * 2;
            }

            public IChildTestClass Create()
            {
                return new ChildTestClass();
            }
        }

        private class TestFactory : IGenericStrategyProvider<TestStrategy, IChildTestClass>
        {
            public TestStrategy GetStrategy(IChildTestClass criterion)
            {
                return new TestStrategy();
            }
        }
        #endregion
    }
}