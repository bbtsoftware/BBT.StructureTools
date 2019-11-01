namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;
    using Xunit.Sdk;

    public class CopyOperationCreateToManyWithReverseRelationIntTests
    {
        #region members and setup
        private readonly ICopy<IParentTestClass> testcandidate;

        public CopyOperationCreateToManyWithReverseRelationIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<IParentTestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass>>().To<ChildTestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<IParentTestClass>>();
        }

        #endregion

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustCopyParentReference()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            var testClassParentCopyChildren = testClassParentCopy.Children.OfType<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            // Don't forget to do this! Since the Children are a Collection the above wy of
            // retrieving the ist is not typesafe and some objects could fall between chairs and table.
            testClassParentCopyChildren.Count.Should().Be(3);

            foreach (var child in testClassParentCopyChildren)
            {
                child.ParentReference.Should().BeSameAs(testClassParentCopy);
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
            // This exception is not originating from XUnit per se,
            // but from the assertion library which is itself using
            // Xunit internally.
            Assert.Throws<XunitException>(() =>
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
        private interface IChildTestClass
        {
            /// <summary>
            /// Gets or sets reference to the parent test class (reverse relation).
            /// </summary>
            IParentTestClass ParentReference { get; set; }
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

            // ReSharper disable once ConvertToAutoProperty it's as intended here.
            // This identifier shall only be used to divide original and copied parent objects when
            // debugging the unit tests.
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
            public IParentTestClass ParentReference { get; set; }
        }

        private class ChildTestClassCopyRegistrations : ICopyRegistrations<IChildTestClass>
        {
            // Further notice: This class is needed and it's registration via IoC container
            // is mandatory. Otherwise copying the child elements wouldn't work!
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<IParentTestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterCreateToManyWithReverseRelation<IChildTestClass, ChildTestClass>(
                    x => x.Children?.Cast<IChildTestClass>(),
                    x => x.Children,
                    x => x.ParentReference);
            }
        }
        #endregion
    }
}