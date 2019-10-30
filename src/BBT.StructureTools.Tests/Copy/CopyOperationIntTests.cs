// Copyright © BBT Software AG. All rights reserved.

using System.Collections.Generic;
using BBT.StructureTools;
using BBT.StructureTools.Copy;
using BBT.StructureTools.Copy.Helper;
using BBT.StructureTools.Copy.Processing;
using BBT.StructureTools.Tests.TestTools;
using FluentAssertions;
using Ninject;
using Xunit;

namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    public class CopyOperationIntTests
    {
        private readonly ICopy<TestClass> testcandidate;

        public CopyOperationIntTests()
        {
            var kernel = TestIoContainer.Initialize();

            kernel.Bind<ICopyRegistrations<TestClass>>().To<TestClassCopyRegistrations>();

            this.testcandidate = kernel.Get<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_WhenAttributeRegistered_MustCopyAttribute()
        {
            // Arrange
            var lSource = new TestClass { Value1 = 42 };
            var lTarget = new TestClass();

            // Act
            this.testcandidate.Copy(lSource, lTarget, new List<IBaseAdditionalProcessing>());

            // Assert
            lTarget.Value1.Should().Be(42);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_WhenAttributeNotRegistered_MustNotCopyAttribute()
        {
            // Arrange
            var lSource = new TestClass { Value2 = 45 };
            var lTarget = new TestClass { Value2 = 35 };

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
            var lSource = new TestClass();
            var lTarget = new TestClass();
            var lAdditionalProcessings = new List<IBaseAdditionalProcessing>
            {
                new GenericCopyPostProcessing<TestClass>((aSource, aTarget) => { aTarget.Value1 = 27; }),
                new GenericCopyPostProcessing<TestClass>((aSource, aTarget) => { aTarget.Value2 = 39; })
            };

            // Act
            this.testcandidate.Copy(lSource, lTarget, lAdditionalProcessings);

            // Assert
            lTarget.Value1.Should().Be(27);
            lTarget.Value2.Should().Be(39);
        }

        private class TestClass
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> aRegistrations)
            {
                aRegistrations.Should().NotBeNull();

                aRegistrations
                    .RegisterAttribute(aX => aX.Value1);
            }
        }
    }
}