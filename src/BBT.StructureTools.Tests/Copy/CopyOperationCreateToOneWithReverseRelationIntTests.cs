namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools.IoC;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [TestFixtureSource(typeof(IocTestFixtureSource), "IocContainers")]
    public class CopyOperationCreateToOneWithReverseRelationIntTests
    {
        #region members and setup
        private readonly ICopy<IParentTestClass> testcandidate;

        public CopyOperationCreateToOneWithReverseRelationIntTests(IIocContainer iocContainer)
        {
            iocContainer.Initialize();

            iocContainer.RegisterSingleton<ICopyRegistrations<IParentTestClass>, TestClassCopyRegistrations>();
            iocContainer.RegisterSingleton<ICopyRegistrations<IChildTestClass>, ChildTestClassCopyRegistrations>();

            this.testcandidate = iocContainer.GetInstance<ICopy<IParentTestClass>>();
        }

        #endregion

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_MustCopyParentReference()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.Child = new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            };

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            // Check if the Child's new reference is correctly set to the copied parent's reference.
            testClassParentCopy.Should().BeSameAs(testClassParentCopy.Child.ParentReference);
        }

        /// <summary>
        /// Tests if the infrastructure can handle a copy create to one with reverse relation
        /// where the child is null.
        /// </summary>
        [Test]
        public void Copy_MustNotFailWhenChildIsNull()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass
            {
                Child = null,
            };

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testcandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>());

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            // Check if the Child of the copied class is null
            testClassParentCopy.Child.Should().BeNull();
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

                registrations.RegisterCreateToOneWithReverseRelation<IChildTestClass, ChildTestClass>(
                    x => x.Child,
                    x => x.ParentReference);
            }
        }
        #endregion
    }
}