// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationCrossReferenceProcessingIntTests
    {
        private readonly ICopy<TestClass> testcandidate;

        /// <summary>
        /// The test fixture set up.
        /// </summary>
        public CopyOperationCrossReferenceProcessingIntTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<TestClassChild>>().To<TestClassChildCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<TestClassCrossReferencedChild>>().To<TestClassCrossReferencedChildCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteAndSetCrossReferenceRegistrations()
        {
            // Arrange
            var lCrossReferenceSource = new TestClassCrossReferencedChild();
            var lSource = new TestClass();
            lCrossReferenceSource.Parent = lSource;
            lSource.TestClassCrossReferencedChild = lCrossReferenceSource;
            lSource.TestClassChild = new TestClassChild
                                         {
                                             Parent = lSource,
                                             CrossReference = lCrossReferenceSource
                                         };
            var lTarget = new TestClass();

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lCrossReferenceSource.Should().NotBeSameAs(lTarget.TestClassCrossReferencedChild);
            lTarget.TestClassCrossReferencedChild.Should().BeSameAs(lTarget.TestClassChild.CrossReference);
        }

        private class TestClass
        {
            public TestClassChild TestClassChild { get; set; }

            public TestClassCrossReferencedChild TestClassCrossReferencedChild { get; set; }
        }

        private class TestClassChild
        {
            public TestClass Parent { get; set; }

            public TestClassCrossReferencedChild CrossReference { get; set; }
        }

        private class TestClassCrossReferencedChild
        {
            public TestClass Parent { get; set; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class instantiated through IOC when IComparer<> is instantiated")]
        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterCrossReferenceProcessing<TestClassCrossReferencedChild, TestClassChild>(
                        aX => aX.CrossReference)
                    .RegisterCreateToOneWithReverseRelation<TestClassCrossReferencedChild, TestClassCrossReferencedChild>(
                        aX => aX.TestClassCrossReferencedChild,
                        aX => aX.Parent)
                    .RegisterCreateToOneWithReverseRelation<TestClassChild, TestClassChild>(
                        aX => aX.TestClassChild,
                        aX => aX.Parent);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class instantiated through IOC when IComparer<> is instantiated")]
        private class TestClassCrossReferencedChildCopyRegistrations : ICopyRegistrations<TestClassCrossReferencedChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassCrossReferencedChild> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class instantiated through IOC when IComparer<> is instantiated")]
        private class TestClassChildCopyRegistrations : ICopyRegistrations<TestClassChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassChild> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();
            }
        }
    }
}