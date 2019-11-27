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
    public class CopyOperationInlineValueProcessingTests
    {
        private readonly ICopy<TestClass> testcandidate;

        public CopyOperationInlineValueProcessingTests(IIocContainer iocContainer)
        {
            iocContainer.Initialize();

            iocContainer.RegisterSingleton<ICopyRegistrations<TestClass>, TestClassCopyRegistrations>();

            this.testcandidate = iocContainer.GetInstance<ICopy<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Test]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var source = new TestClass();
            var target = new TestClass();

            // Act
            this.testcandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.TestGuid.Should().NotBe(Guid.Empty);
        }

        private class TestClass
        {
            public Guid TestGuid { get; set; } = Guid.Empty;
        }

        private class TestClassCopyRegistrations : ICopyRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterInlineValueProcessing(x => x.TestGuid, () => Guid.NewGuid());
            }
        }
    }
}