// Copyright © BBT Software AG. All rights reserved.

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationInlineValueProcessingTests
    {
        private readonly ICopy<TestClass> testcandidate;

        public CopyOperationInlineValueProcessingTests()
        {
            var kernel = Setup.SetUpIocResolve();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var lSource = new TestClass();
            var lTarget = new TestClass();

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lTarget.TestGuid.Should().NotBe(Guid.Empty);
        }

        private class TestClass
        {
            public Guid TestGuid { get; set; } = Guid.Empty;
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterInlineValueProcessing(aX => aX.TestGuid, () => Guid.NewGuid());
            }
        }
    }
}