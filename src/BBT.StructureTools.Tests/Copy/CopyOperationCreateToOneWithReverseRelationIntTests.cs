// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationCreateToOneWithReverseRelationIntTests
    {
        #region members and setup
        private readonly ICopy<IParentTestClass> testcandidate;

        /// <summary>
        /// The test fixture set up.
        /// </summary>
        public CopyOperationCreateToOneWithReverseRelationIntTests()
        {
            var kernel = Setup.SetUpIocResolve();

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
            var lTestClassParentOriginal = new ParentTestClass();
            lTestClassParentOriginal.Child = new ChildTestClass()
            {
                ParentReference = lTestClassParentOriginal
            };

            var lTestClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                lTestClassParentOriginal,
                lTestClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            lTestClassParentCopy.Should().NotBeSameAs(lTestClassParentOriginal);

            // Check if the Child's new reference is correctly set to the copied parent's reference.
            lTestClassParentCopy.Should().BeSameAs(lTestClassParentCopy.Child.ParentReference);
        }

        /// <summary>
        /// Tests if the infrastructure can handle a copy create to one with reverse relation
        /// where the child is null.
        /// </summary>
        [Fact]
        public void Copy_MustNotFailWhenChildIsNull()
        {
            // Arrange
            var lTestClassParentOriginal = new ParentTestClass
            {
                Child = null
            };

            var lTestClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                lTestClassParentOriginal,
                lTestClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            lTestClassParentCopy.Should().NotBeSameAs(lTestClassParentOriginal);

            // Check if the Child of the copied class is null
            lTestClassParentCopy.Child.Should().BeNull();
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
            IChildTestClass Child { get; set; }

            /// <summary>
            /// Gets Identifier (use for debug).
            /// </summary>
            Guid Identifier { get; }
        }

        private class ParentTestClass : IParentTestClass
        {
            public IChildTestClass Child { get; set; }

            public Guid Identifier { get; } = Guid.NewGuid();
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

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class instantiated through IOC when IComparer<> is instantiated")]
        private class TestClassCopyRegistrations : ICopyRegistrations<IParentTestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<IParentTestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations.RegisterCreateToOneWithReverseRelation<IChildTestClass, ChildTestClass>(
                    aX => aX.Child,
                    aX => aX.ParentReference);
            }
        }
        #endregion
    }
}