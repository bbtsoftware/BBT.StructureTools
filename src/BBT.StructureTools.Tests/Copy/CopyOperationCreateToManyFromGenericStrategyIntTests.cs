// Copyright © BBT Software AG. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public class CopyOperationCreateToManyFromGenericStrategyIntTests
    {
        #region setup

        private readonly ICopy<ParentTestClass> testcandidate;


        public CopyOperationCreateToManyFromGenericStrategyIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<IParentTestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass>>().To<ChildTestClassCopyRegistrations>();
            kernel.Bind<IGenericStrategyProvider<TestStrategy, IChildTestClass>>().To<TestFactory>();
            kernel.Bind<ITestStrategy>().To<TestStrategy>();

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
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustFailWhenChildrenListNull()
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
        [Fact]
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
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Suppression is OK here.")]
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
            /// <param name="aChild">child item.</param>
            void AddChild(ChildTestClass aChild);

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
            private readonly Guid mIdentifier = Guid.NewGuid();

            public IList<IChildTestClass> Children { get; set; } = new List<IChildTestClass>();

            // ReSharper disable once ConvertToAutoProperty it's as intended here.
            // This identifier shall only be used to divide original and copied parent objects when
            // debugging the unit tests.
            public Guid Identifier => this.mIdentifier;

            public void AddChild(ChildTestClass aChild)
            {
                this.Children.Add(aChild);
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
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();
            }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<IParentTestClass>
        {
            public TestClassCopyRegistrations()
            {
            }

            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();
                aRegistrations.RegisterCreateToManyFromGenericStrategy<ITestStrategy, IChildTestClass>(
                    aX => aX.Children.Cast<IChildTestClass>(),
                    aX => aX.Children,
                    aX => aX.CreateTestClass());
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
            public bool IsResponsible(IChildTestClass aCriterion)
            {
                // For Test purposes.
                return true;
            }

            public IChildTestClass CreateTestClass() => new ChildTestClass();

            public IChildTestClass Create()
            {
                return new ChildTestClass();
            }

            public void Copy(IChildTestClass aSource, IChildTestClass aTarget, ICopyCallContext aCopyCalcontext)
            {
                aSource.Should().NotBeNull();
                aTarget.Should().NotBeNull();
                aCopyCalcontext.Should().NotBeNull();

                aTarget.TestValue = aSource.TestValue * 2;
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