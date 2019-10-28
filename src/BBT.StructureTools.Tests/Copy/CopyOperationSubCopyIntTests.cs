// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationSubCopyIntTests
    {
        private readonly ICopy<TestClassChild> testcandidate;

        public CopyOperationSubCopyIntTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICopyRegistrations<TestClassChild>>().To<TestClassChildCopyRegistrations>();
            kernel.Bind<ICopyRegistrations<TestClassParent>>().To<TestClassParentCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<TestClassChild>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_WhenAttributeRegistered_MustCopyAttribute()
        {
            // Arrange
            var lSource = new TestClassChild { Value1 = 45 };
            var lTarget = new TestClassChild();

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lTarget.Value1.Should().Be(45);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_WhenAttributeNotRegistered_MustNotCopyAttribute()
        {
            // Arrange
            var lSource = new TestClassChild { Value2 = 45 };
            var lTarget = new TestClassChild { Value2 = 35 };

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lTarget.Value2.Should().Be(35);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustExecuteAdditionalProcessings()
        {
            // Arrange
            var lSource = new TestClassChild();
            var lTarget = new TestClassChild();
            var lAdditionalProcessings = new List<IBaseAdditionalProcessing>
            {
                new GenericCopyPostProcessing<TestClassParent>((aSource, aTarget) => { aTarget.Value1 = 27; }),
                new GenericCopyPostProcessing<TestClassParent>((aSource, aTarget) => { aTarget.Value2 = 39; })
            };

            // Act
            this.testcandidate.Copy(lSource, lTarget, lAdditionalProcessings);

            // Assert
            lTarget.Value1.Should().Be(27);
            lTarget.Value2.Should().Be(39);
        }

        private class TestClassChild : TestClassParent
        {
        }

        private class TestClassParent
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestClassChildCopyRegistrations : ICopyRegistrations<TestClassChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassChild> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterSubCopy<ICopy<TestClassParent>>();
            }
        }

        private class TestClassParentCopyRegistrations : ICopyRegistrations<TestClassParent>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassParent> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterAttribute(aX => aX.Value1);
            }
        }
    }
}