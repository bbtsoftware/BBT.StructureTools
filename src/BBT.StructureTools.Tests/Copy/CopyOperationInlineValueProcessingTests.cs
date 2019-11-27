namespace BBT.StructureTools.Tests.Copy
{
    using System;
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationInlineValueProcessingTests
    {
        private readonly ICopier<TestClass> testCandidate;

        public CopyOperationInlineValueProcessingTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<ICopierRegistrations<TestClass>>().To<TestClassCopierRegistrations>();

            this.testCandidate = kernel.Get<ICopier<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteInlineCopyProcessing()
        {
            // Arrange
            var source = new TestClass();
            var target = new TestClass();

            // Act
            this.testCandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            target.TestGuid.Should().NotBe(Guid.Empty);
        }

        private class TestClass
        {
            public Guid TestGuid { get; set; } = Guid.Empty;
        }

        private class TestClassCopierRegistrations : ICopierRegistrations<TestClass>
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