namespace BBT.StructureTools.Tests.Copy
{
    using System.Collections.Generic;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Tests.TestTools;
    using FluentAssertions;
    using Ninject;
    using Xunit;

    public class CopyOperationCrossReferenceProcessingIntTests
    {
        private readonly ICopier<TestClass> testCandidate;

        public CopyOperationCrossReferenceProcessingIntTests()
        {
            var kernel = TestIocContainer.Initialize();

            kernel.Bind<ICopierRegistrations<TestClass>>().To<TestClassCopierRegistrations>();
            kernel.Bind<ICopierRegistrations<TestClassChild>>().To<TestClassChildCopierRegistrations>();
            kernel.Bind<ICopierRegistrations<TestClassCrossReferencedChild>>().To<TestClassCrossReferencedChildCopierRegistrations>();

            this.testCandidate = kernel.Get<ICopier<TestClass>>();
        }

        /// <summary>
        /// Tests ICopy.Copy.
        /// </summary>
        [Fact]
        public void MustExecuteAndSetCrossReferenceRegistrations()
        {
            // Arrange
            var crossReferenceSource = new TestClassCrossReferencedChild();
            var source = new TestClass();
            crossReferenceSource.Parent = source;
            source.TestClassCrossReferencedChild = crossReferenceSource;
            source.TestClassChild = new TestClassChild
            {
                Parent = source,
                CrossReference = crossReferenceSource,
            };
            var target = new TestClass();

            // Act
            this.testCandidate.Copy(source, target, new List<IBaseAdditionalProcessing>());

            // Assert
            crossReferenceSource.Should().NotBeSameAs(target.TestClassCrossReferencedChild);
            target.TestClassCrossReferencedChild.Should().BeSameAs(target.TestClassChild.CrossReference);
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

        private class TestClassCopierRegistrations : ICopierRegistrations<TestClass>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClass> registrations)
            {
                registrations.Should().NotBeNull();

                registrations
                    .RegisterCrossReferenceProcessing<TestClassCrossReferencedChild, TestClassChild>(
                        x => x.CrossReference)
                    .RegisterCreateToOneWithReverseRelation<TestClassCrossReferencedChild, TestClassCrossReferencedChild>(
                        x => x.TestClassCrossReferencedChild,
                        x => x.Parent)
                    .RegisterCreateToOneWithReverseRelation<TestClassChild, TestClassChild>(
                        x => x.TestClassChild,
                        x => x.Parent);
            }
        }

        private class TestClassCrossReferencedChildCopierRegistrations : ICopierRegistrations<TestClassCrossReferencedChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassCrossReferencedChild> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }

        private class TestClassChildCopierRegistrations : ICopierRegistrations<TestClassChild>
        {
            public void DoRegistrations(ICopyHelperRegistration<TestClassChild> registrations)
            {
                registrations.Should().NotBeNull();
            }
        }
    }
}