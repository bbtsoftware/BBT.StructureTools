// Copyright © BBT Software AG. All rights reserved.

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

namespace BBT.StructureTools.Tests.Copy
{
    public class CopyOperationCreateToManyFromGenericStrategyWithReverseRelationIntTests
    {
        #region setup

        private readonly ICopy<IParentTestClass> testcandidate;

        public CopyOperationCreateToManyFromGenericStrategyWithReverseRelationIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<IGenericStrategyProvider<TestStrategy, IChildTestClass>>().To<TestFactory>();
            kernel.Bind<ITestStrategy>().To<TestStrategy>();
            kernel.Bind<ICopyRegistrations<IParentTestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass>>().To<ChildTestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<IParentTestClass>>();
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
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            var testClassParentCopyChildren = testClassParentCopy.Children.Cast<ChildTestClass>().ToList();
            var testClassParentOriginachildren = testClassParentOriginal.Children.Cast<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            testClassParentCopyChildren.Count.Should().Be(3);

            testClassParentCopyChildren.ElementAt(0).TestValue.Should().Be(testClassParentOriginachildren.ElementAt(0).TestValue * 2);
            testClassParentCopyChildren.ElementAt(1).TestValue.Should().Be(testClassParentOriginachildren.ElementAt(1).TestValue * 2);
            testClassParentCopyChildren.ElementAt(2).TestValue.Should().Be(testClassParentOriginachildren.ElementAt(2).TestValue * 2);

            foreach (var copiedChild in testClassParentCopyChildren)
            {
                foreach (var lOriginachild in testClassParentOriginal.Children)
                {
                    lOriginachild.Should().NotBeSameAs(copiedChild);
                }
            }

            // Check for the correct referential reverse relation copy.
            foreach (var lChild in testClassParentCopyChildren)
            {
                lChild.Parent.Should().BeSameAs(testClassParentCopy);
            }
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustFailWhenChildrenListNull()
        {
            // Arrange
            var lTestClassParentOriginal = new ParentTestClass();
            lTestClassParentOriginal.MakeChildrenCollectionNull();

            var lTestClassParentCopy = new ParentTestClass();

            // Act / Assert throws
            Assert.Throws<ArgumentNullException>(() =>
                this.testcandidate.Copy(
                    lTestClassParentOriginal,
                    lTestClassParentCopy,
                    new List<IBaseAdditionalProcessing>()));
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustCopyEmptyCollection()
        {
            // Arrange
            var lTestClassParentOriginal = new ParentTestClass();

            var lTestClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                lTestClassParentOriginal,
                lTestClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            // Also make sure the parent test class children collection, which is empty, was copied.
            lTestClassParentCopy.Should().NotBeSameAs(lTestClassParentOriginal);
            lTestClassParentCopy.Children.Should().NotBeSameAs(lTestClassParentOriginal.Children);
            lTestClassParentCopy.Should().NotBeNull();
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
            /// <param name="aChild">child item.</param>
            void AddChild(ChildTestClass aChild);

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
            private readonly Guid mIdentifier = Guid.NewGuid();

            public ICollection<IChildTestClass> Children { get; set; } = new List<IChildTestClass>();

            // ReSharper disable once ConvertToAutoProperty it's as intended here.
            // This identifier shall only be used to divide original and copied parent objects when
            // debugging the unit tests.
            public Guid Identifier => this.mIdentifier;

            public void AddChild(ChildTestClass aChild)
            {
                this.Children.Add(aChild);
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

        private class ChildTestClassCopyRegistrations : ICopyRegistrations<IChildTestClass>
        {
            // Further notice: This class is needed and it's registration via IoC container
            // is mandatory. Otherwise copying the child elements wouldn't work!
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();
            }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<IParentTestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterCreateToManyFromGenericStrategyWithReverseRelation<ITestStrategy, IChildTestClass>(
                    aX => aX.Children.Cast<IChildTestClass>(),
                    aX => aX.Children,
                    aX => aX.Parent);
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
            public bool IsResponsible(IChildTestClass aCriterion)
            {
                // For Test purposes.
                return true;
            }

            public void Copy(IChildTestClass aSource, IChildTestClass aTarget, ICopyCallContext aCopyCallContext)
            {
                aSource.Should().NotBeNull();
                aTarget.Should().NotBeNull();
                aCopyCallContext.Should().NotBeNull();

                aTarget.TestValue = aSource.TestValue * 2;
            }

            public IChildTestClass Create()
            {
                return new ChildTestClass();
            }
        }

        private class TestFactory : IGenericStrategyProvider<TestStrategy, IChildTestClass>
        {
            public IEnumerable<TestStrategy> GetAllStrategies()
            {
                // Not needed for test scenario
                throw new NotImplementedException();
            }

            public TestStrategy GetStrategy(IChildTestClass aCriterion)
            {
                return new TestStrategy();
            }
        }
        #endregion
    }
}