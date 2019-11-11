namespace BBT.StructureTools.Tests.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopierIntegrationTests
    {
        #region members and setup
        private readonly ICopy<ParentTestClass> testCandidate;

        public CopierIntegrationTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<IParentTestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass>>().To<ChildTestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<IChildTestClass2>>().To<ChildTestClass2CopyRegistrations>();

            this.testCandidate = kernel.Get<ICopy<IParentTestClass>>();
        }

        #endregion

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustNotCopyChildrenWhenIntercepted()
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
            this.testCandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>
                {
                    new GenericContinueCopyInterception<IChildTestClass>(obj => false),
                });

            var testClassParentCopyChildren = testClassParentCopy.Children.Cast<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            testClassParentCopyChildren.Count.Should().Be(0);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustCopyInheritedChildrenWhenIntercepted()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass2()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testCandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>
                {
                    new GenericContinueCopyInterception<IChildTestClass2>(obj => false),
                });

            var testClassParentCopyChildren = testClassParentCopy.Children.Cast<ChildTestClass>().ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            testClassParentOriginal.Children.Count().Should().Be(3);

            foreach (var childhild in testClassParentCopyChildren)
            {
                childhild.ParentReference.Should().BeSameAs(testClassParentCopy);
            }
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustNotCopyInheritedChildrenAndChildrenWhenChildrenIntercepted()
        {
            // Arrange
            var testClassParentOriginal = new ParentTestClass();
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass2()
            {
                ParentReference = testClassParentOriginal,
            });
            testClassParentOriginal.AddChild(new ChildTestClass()
            {
                ParentReference = testClassParentOriginal,
            });

            var testClassParentCopy = new ParentTestClass();

            // Act
            this.testCandidate.Copy(
                testClassParentOriginal,
                testClassParentCopy,
                new List<IBaseAdditionalProcessing>
                {
                    new GenericContinueCopyInterception<IChildTestClass>(obj => false),
                });

            var testClassParentCopyChildren = testClassParentCopy.Children.ToList();

            // Assert
            // Make sure original and copy of the parent object are not the same.
            testClassParentCopy.Should().NotBeSameAs(testClassParentOriginal);

            testClassParentCopyChildren.Count.Should().Be(0);

            foreach (var childhild in testClassParentCopyChildren)
            {
                childhild.ParentReference.Should().BeSameAs(testClassParentCopy);
            }
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
        /// An interface declaring an inherited child test class.
        /// </summary>
        private interface IChildTestClass2 : IChildTestClass
        {
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
            public IParentTestClass ParentReference { get; set; }
        }

        private class ChildTestClass2 : ChildTestClass, IChildTestClass2
        {
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

        private class ChildTestClass2CopyRegistrations : ICopyRegistrations<IChildTestClass2>
        {
            public void DoRegistrations(ICopyHelperRegistration<IChildTestClass2> registrations)
            {
                registrations.Should().NotBeNull();

                registrations.RegisterSubCopy<ICopy<IChildTestClass>>();
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