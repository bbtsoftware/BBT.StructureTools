// Copyright © BBT Software AG. All rights reserved.

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

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    public class CopyOperationCreateToManyWithReverseRelationIntTests
    {
        #region members and setup / teardown of the tests
        private readonly ICopy<IParentTestClass> testcandidate;

        /// <summary>
        /// The test fixture set up.
        /// </summary>
        public CopyOperationCreateToManyWithReverseRelationIntTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICopyRegistrations<IParentTestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass>>().To<ChildTestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<IParentTestClass>>();
        }

        #endregion

        #region test cases

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustCopyParentReference()
        {
            // Arrange
            var lTestClassParentOriginal = new ParentTestClass();
            lTestClassParentOriginal.AddChild(new ChildTestClass()
                                                  {
                                                      ParentReference = lTestClassParentOriginal
                                                  });
            lTestClassParentOriginal.AddChild(new ChildTestClass()
                                                  {
                                                      ParentReference = lTestClassParentOriginal
                                                  });
            lTestClassParentOriginal.AddChild(new ChildTestClass()
                                                  {
                                                      ParentReference = lTestClassParentOriginal
                                                  });

            var lTestClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                lTestClassParentOriginal,
                lTestClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            var lTestClassParentCopyChildren = lTestClassParentCopy.Children.OfType<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            lTestClassParentCopy.Should().NotBeSameAs(lTestClassParentOriginal);

            // Don't forget to do this! Since the Children are a Collection the above way of
            // retrieving the list is not typesafe and some objects could fall between chairs and table.
            lTestClassParentCopyChildren.Count.Should().Be(3);

            foreach (var lChild in lTestClassParentCopyChildren)
            {
                lChild.ParentReference.Should().BeSameAs(lTestClassParentCopy);
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
            // This exception is not originating from XUnit per se,
            // but from the assertion library which is itself using
            // Xunit internally.
            Assert.Throws<XunitException>(() =>
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
            lTestClassParentCopy.Children.Should().NotBeNull();
        }

        #endregion

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
            public IParentTestClass ParentReference { get; set; }
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

                aRegistrations.RegisterCreateToManyWithReverseRelation<IChildTestClass, ChildTestClass>(
                    aX => aX.Children?.Cast<IChildTestClass>(),
                    aX => aX.Children,
                    aX => aX.ParentReference);
            }
        }
        #endregion
    }
}