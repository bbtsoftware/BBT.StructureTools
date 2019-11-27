namespace BBT.StructureTools.Tests.Convert
{
    using System.Collections.Generic;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Tests.Convert.TestStructure.Source;
    using BBT.StructureTools.Tests.Convert.TestStructure.Target;
    using Moq;
    using NUnit.Framework;

    public class ConvertHelperTests
    {
        [Test]
        public void PreProcessing_WithEmptyist_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();
            var target = new TargetRoot();

            testcandidate.DoConvertPreProcessing(source, target, preprocessings);
        }

        [Test]
        public void PreProcessing_ExecutesPreprocessing()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var preprocShouldExecute = new Mock<IConvertPreProcessing<Root, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPreProcessing<Leaf, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPostProcessing<Root, TargetRoot>>();

            preprocShouldExecute.Setup(x => x.DoPreProcessing(source, target));
            preprocShouldNotExecuteBecauseWrongSource.Setup(x => x.DoPreProcessing(It.IsAny<Leaf>(), target));
            preprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPostProcessing(source, target));

            var preprocessings = new List<IBaseAdditionalProcessing>
            {
                preprocShouldExecute.Object,
                preprocShouldNotExecuteBecauseWrongSource.Object,
                preprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testcandidate.DoConvertPreProcessing(source, target, preprocessings);

            preprocShouldExecute.Verify(x => x.DoPreProcessing(source, target), Times.Once);
            preprocShouldNotExecuteBecauseWrongSource.Verify(x => x.DoPreProcessing(It.IsAny<Leaf>(), target), Times.Never);
            preprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPostProcessing(source, target), Times.Never);
        }

        [Test]
        public void PostProcessing_WithEmptyist_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();
            var target = new TargetRoot();

            testcandidate.DoConvertPostProcessing(source, target, preprocessings);
        }

        [Test]
        public void PostProcessing_ExecutesPostprocessing()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertPostProcessing<Root, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPostProcessing<Leaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<Root, TargetRoot>>();

            postprocShouldExecute.Setup(x => x.DoPostProcessing(source, target));
            postprocShouldNotExecuteBecauseWrongSource.Setup(x => x.DoPostProcessing(It.IsAny<Leaf>(), target));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testcandidate.DoConvertPostProcessing(source, target, postprocessings);

            postprocShouldExecute.Verify(x => x.DoPostProcessing(source, target), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(x => x.DoPostProcessing(It.IsAny<Leaf>(), target), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPreProcessing(source, target), Times.Never);
        }

        [Test]
        public void Intercept_WithEmptyist_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();

            testcandidate.ContinueConvertProcess<Root, TargetRoot>(source, preprocessings);
        }

        [Test]
        public void ConvertInterception_Intercepts()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertInterception<Root, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertInterception<Leaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<Root, TargetRoot>>();

            postprocShouldExecute.Setup(x => x.CallConverter(source));
            postprocShouldNotExecuteBecauseWrongSource.Setup(x => x.CallConverter(It.IsAny<Leaf>()));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(x => x.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object,
            };

            testcandidate.ContinueConvertProcess<Root, TargetRoot>(source, postprocessings);

            postprocShouldExecute.Verify(x => x.CallConverter(source), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(x => x.CallConverter(It.IsAny<Leaf>()), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(x => x.DoPreProcessing(source, target), Times.Never);
        }
    }
}
