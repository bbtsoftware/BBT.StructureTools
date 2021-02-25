namespace BBT.StructureTools.Tests.Convert
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;
    using Moq;
    using Xunit;

    public class ConvertHelperTests
    {
        [Fact]
        public void PreProcessing_WithEmptyist_Succeeds()
        {
            var testCandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new SourceRoot();
            var target = new TargetRoot();

            testCandidate.DoConvertPreProcessing(source, target, preprocessings);
        }

        [Fact]
        public void PreProcessing_ExecutesPreprocessing()
        {
            var testCandidate = new ConvertHelper();

            var source = new SourceRoot();
            var target = new TargetRoot();

            var preprocShouldExecute = new Mock<IConvertPreProcessing<SourceRoot, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPreProcessing<SourceTreeLeaf, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPostProcessing<SourceRoot, TargetRoot>>();

            preprocShouldExecute.Setup(x => x.DoPreProcessing(source, target));
            preprocShouldNotExecuteBecauseWrongSource.Setup(x => x.DoPreProcessing(It.IsAny<SourceTreeLeaf>(), target));
            preprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPostProcessing(source, target));

            var preprocessings = new List<IBaseAdditionalProcessing>
            {
                preprocShouldExecute.Object,
                preprocShouldNotExecuteBecauseWrongSource.Object,
                preprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testCandidate.DoConvertPreProcessing(source, target, preprocessings);

            preprocShouldExecute.Verify(x => x.DoPreProcessing(source, target), Times.Once);
            preprocShouldNotExecuteBecauseWrongSource.Verify(x => x.DoPreProcessing(It.IsAny<SourceTreeLeaf>(), target), Times.Never);
            preprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPostProcessing(source, target), Times.Never);
        }

        [Fact]
        public void PostProcessing_WithEmptyist_Succeeds()
        {
            var testCandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new SourceRoot();
            var target = new TargetRoot();

            testCandidate.DoConvertPostProcessing(source, target, preprocessings);
        }

        [Fact]
        public void PostProcessing_ExecutesPostprocessing()
        {
            var testCandidate = new ConvertHelper();

            var source = new SourceRoot();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertPostProcessing<SourceRoot, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPostProcessing<SourceTreeLeaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<SourceRoot, TargetRoot>>();

            postprocShouldExecute.Setup(x => x.DoPostProcessing(source, target));
            postprocShouldNotExecuteBecauseWrongSource.Setup(x => x.DoPostProcessing(It.IsAny<SourceTreeLeaf>(), target));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testCandidate.DoConvertPostProcessing(source, target, postprocessings);

            postprocShouldExecute.Verify(x => x.DoPostProcessing(source, target), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(x => x.DoPostProcessing(It.IsAny<SourceTreeLeaf>(), target), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPreProcessing(source, target), Times.Never);
        }

        [Fact]
        public void Intercept_WithEmptyist_Succeeds()
        {
            var testCandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new SourceRoot();

            testCandidate.ContinueConvertProcess<SourceRoot, TargetRoot>(source, preprocessings);
        }

        [Fact]
        public void ConvertInterception_Intercepts()
        {
            var testCandidate = new ConvertHelper();

            var source = new SourceRoot();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertInterception<SourceRoot, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertInterception<SourceTreeLeaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<SourceRoot, TargetRoot>>();

            postprocShouldExecute.Setup(x => x.CallConverter(source));
            postprocShouldNotExecuteBecauseWrongSource.Setup(x => x.CallConverter(It.IsAny<SourceTreeLeaf>()));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testCandidate.ContinueConvertProcess<SourceRoot, TargetRoot>(source, postprocessings);

            postprocShouldExecute.Verify(x => x.CallConverter(source), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(x => x.CallConverter(It.IsAny<SourceTreeLeaf>()), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPreProcessing(source, target), Times.Never);
        }
    }
}
