namespace BBT.Life.LiBase.ITests.General.Services.Tools.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Processing;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CopyOperationSubCopyIntTests
    {
        private readonly ICopy<TestClassChild> testcandidate;

        public CopyOperationSubCopyIntTests()
        {
            var kernel = new NinjectIocContainer();

            kernel.RegisterSingleton<ICopyRegistrations<TestClassChild>, TestClassChildCopyRegistrations>();
            kernel.RegisterSingleton<ICopyRegistrations<TestClassParent>, TestClassParentCopyRegistrations>();

            this.testcandidate = kernel.GetInstance<ICopy<TestClassChild>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_WhenAttributeRegistered_MustCopyAttribute()
        {
            // Arrange
            var source = new TestClassChild { Value1 = 45 };
            var target = new TestClassChild();

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value1.Should().Be(45);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_WhenAttributeNotRegistered_MustNotCopyAttribute()
        {
            // Arrange
            var source = new TestClassChild { Value2 = 45 };
            var target = new TestClassChild { Value2 = 35 };

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.Value2.Should().Be(35);
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void Copy_MustExecuteAdditionalProcessings()
        {
            // Arrange
            var source = new TestClassChild();
            var target = new TestClassChild();
            var additionalProcessings = new List<IBaseAdditionalProcessing>
            {
                new GenericCopyPostProcessing<TestClassParent>((sourceX, targetX) => { target.Value1 = 27; }),
                new GenericCopyPostProcessing<TestClassParent>((sourceX, targetX) => { target.Value2 = 39; }),
            };

            // Act
            this.testcandidate.Copy(source, target, additionalProcessings);

            // Assert
            target.Value1.Should().Be(27);
            target.Value2.Should().Be(39);
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
            public void DoRegistrations(ICopyHelperRegistration<TestClassChild> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterSubCopy<ICopy<TestClassParent>>();
            }
        }

        private class TestClassParentCopyRegistrations : ICopyRegistrations<TestClassParent>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassParent> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterAttribute(x => x.Value1);
            }
        }
    }
}