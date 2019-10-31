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
            var source = new TestClass { Value1 = 42 };
            var target = new TestClass();

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value1.Should().Be(42);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_WhenAttributeNotRegistered_MustNotCopyAttribute()
        {
            // Arrange
            var source = new TestClass { Value2 = 45 };
            var target = new TestClass { Value2 = 35 };

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value2.Should().Be(35);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void Copy_MustExecuteAdditionalProcessings()
        {
            // Arrange
            var source = new TestClass();
            var target = new TestClass();
            var additionalProcessings = new List<IBaseAdditionalProcessing>
            {
                new GenericCopyPostProcessing<TestClass>((sourceX, targetX) => { target.Value1 = 27; }),
                new GenericCopyPostProcessing<TestClass>((sourceX, targetX) => { target.Value2 = 39; }),
            };

            // Act
            this.testcandidate.Copy(source, target, additionalProcessings);

            // Assert
            target.Value1.Should().Be(27);
            target.Value2.Should().Be(39);
        }

        private class TestClass
        {
            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterAttribute(x => x.Value1);
            }
        }
    }
}