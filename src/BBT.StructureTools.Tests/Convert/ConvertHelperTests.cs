namespace BBT.StructureTools.Tests.Convert
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.TestData;
    using NSubstitute;
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

            var preprocShouldExecute = Substitute.For<IConvertPreProcessing<SourceRoot, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongSource = Substitute.For<IConvertPreProcessing<SourceTreeLeaf, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongProcessingType = Substitute.For<IConvertPostProcessing<SourceRoot, TargetRoot>>();

            var preprocessings = new List<IBaseAdditionalProcessing>
            {
                preprocShouldExecute,
                preprocShouldNotExecuteBecauseWrongSource,
                preprocShouldNotExecuteBecauseWrongProcessingType,
            };

            testCandidate.DoConvertPreProcessing(source, target, preprocessings);

            preprocShouldExecute.Received(1).DoPreProcessing(source, target);
            preprocShouldNotExecuteBecauseWrongSource.DidNotReceive().DoPreProcessing(Arg.Any<SourceTreeLeaf>(), target);
            preprocShouldNotExecuteBecauseWrongProcessingType.DidNotReceive().DoPostProcessing(source, target);
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

            var postprocShouldExecute = Substitute.For<IConvertPostProcessing<SourceRoot, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = Substitute.For<IConvertPostProcessing<SourceTreeLeaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = Substitute.For<IConvertPreProcessing<SourceRoot, TargetRoot>>();

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute,
                postprocShouldNotExecuteBecauseWrongSource,
                postprocShouldNotExecuteBecauseWrongProcessingType,
            };

            testCandidate.DoConvertPostProcessing(source, target, postprocessings);

            postprocShouldExecute.Received(1).DoPostProcessing(source, target);
            postprocShouldNotExecuteBecauseWrongSource.DidNotReceive().DoPostProcessing(Arg.Any<SourceTreeLeaf>(), target);
            postprocShouldNotExecuteBecauseWrongProcessingType.DidNotReceive().DoPreProcessing(source, target);
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

            var postprocShouldExecute = Substitute.For<IConvertInterception<SourceRoot, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = Substitute.For<IConvertInterception<SourceTreeLeaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = Substitute.For<IConvertPreProcessing<SourceRoot, TargetRoot>>();

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute,
                postprocShouldNotExecuteBecauseWrongSource,
                postprocShouldNotExecuteBecauseWrongProcessingType,
            };

            testCandidate.ContinueConvertProcess<SourceRoot, TargetRoot>(source, postprocessings);

            postprocShouldExecute.Received(1).CallConverter(source);
            postprocShouldNotExecuteBecauseWrongSource.DidNotReceive().CallConverter(Arg.Any<SourceTreeLeaf>());
            postprocShouldNotExecuteBecauseWrongProcessingType.DidNotReceive().DoPreProcessing(source, target);
        }
    }
}
