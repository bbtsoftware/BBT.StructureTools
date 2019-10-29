using System.Collections.Generic;
using BBT.StructureTools.Convert;
using BBT.StructureTools.Tests.Convert.TestStructure.Source;
using BBT.StructureTools.Tests.Convert.TestStructure.Target;
using Moq;
using Xunit;

namespace BBT.StructureTools.Tests.Convert
{
    public class ConvertHelperTests
    {
        [Fact]
        public void PreProcessing_WithEmptyList_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();
            var target = new TargetRoot();

            testcandidate.DoConvertPreProcessing(source, target, preprocessings);
        }

        [Fact]
        public void PreProcessing_ExecutesPreprocessing()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var preprocShouldExecute = new Mock<IConvertPreProcessing<Root, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPreProcessing<Leaf, TargetRoot>>();
            var preprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPostProcessing<Root, TargetRoot>>();

            preprocShouldExecute.Setup(aX => aX.DoPreProcessing(source, target));
            preprocShouldNotExecuteBecauseWrongSource.Setup(aX => aX.DoPreProcessing(It.IsAny<Leaf>(), target));
            preprocShouldNotExecuteBecauseWrongProcessingType.Setup(aX => aX.DoPostProcessing(source, target));

            var preprocessings = new List<IBaseAdditionalProcessing>
            {
                preprocShouldExecute.Object,
                preprocShouldNotExecuteBecauseWrongSource.Object,
                preprocShouldNotExecuteBecauseWrongProcessingType.Object
            };


            testcandidate.DoConvertPreProcessing(source, target, preprocessings);

            preprocShouldExecute.Verify(aX => aX.DoPreProcessing(source, target), Times.Once);
            preprocShouldNotExecuteBecauseWrongSource.Verify(aX => aX.DoPreProcessing(It.IsAny<Leaf>(), target), Times.Never);
            preprocShouldNotExecuteBecauseWrongProcessingType.Verify(aX => aX.DoPostProcessing(source, target), Times.Never);
        }

        [Fact]
        public void PostProcessing_WithEmptyList_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();
            var target = new TargetRoot();

            testcandidate.DoConvertPostProcessing(source, target, preprocessings);
        }

        [Fact]
        public void PostProcessing_ExecutesPostprocessing()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertPostProcessing<Root, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertPostProcessing<Leaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<Root, TargetRoot>>();

            postprocShouldExecute.Setup(aX => aX.DoPostProcessing(source, target));
            postprocShouldNotExecuteBecauseWrongSource.Setup(aX => aX.DoPostProcessing(It.IsAny<Leaf>(), target));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(aX => aX.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object
            };


            testcandidate.DoConvertPostProcessing(source, target, postprocessings);

            postprocShouldExecute.Verify(aX => aX.DoPostProcessing(source, target), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(aX => aX.DoPostProcessing(It.IsAny<Leaf>(), target), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(aX => aX.DoPreProcessing(source, target), Times.Never);
        }


        [Fact]
        public void Intercept_WithEmptyList_Succeeds()
        {
            var testcandidate = new ConvertHelper();
            var preprocessings = new List<IBaseAdditionalProcessing>();

            var source = new Root();

            testcandidate.ContinueConvertProcess<Root, TargetRoot>(source, preprocessings);
        }

        [Fact]
        public void ConvertInterception_Intercepts()
        {
            var testcandidate = new ConvertHelper();

            var source = new Root();
            var target = new TargetRoot();

            var postprocShouldExecute = new Mock<IConvertInterception<Root, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongSource = new Mock<IConvertInterception<Leaf, TargetRoot>>();
            var postprocShouldNotExecuteBecauseWrongProcessingType = new Mock<IConvertPreProcessing<Root, TargetRoot>>();

            postprocShouldExecute.Setup(aX => aX.CallConverter(source));
            postprocShouldNotExecuteBecauseWrongSource.Setup(aX => aX.CallConverter(It.IsAny<Leaf>()));
            postprocShouldNotExecuteBecauseWrongProcessingType.Setup(aX => aX.DoPreProcessing(source, target));

            var postprocessings = new List<IBaseAdditionalProcessing>
            {
                postprocShouldExecute.Object,
                postprocShouldNotExecuteBecauseWrongSource.Object,
                postprocShouldNotExecuteBecauseWrongProcessingType.Object
            };


            testcandidate.ContinueConvertProcess<Root, TargetRoot>(source, postprocessings);

            postprocShouldExecute.Verify(aX => aX.CallConverter(source), Times.Once);
            postprocShouldNotExecuteBecauseWrongSource.Verify(aX => aX.CallConverter(It.IsAny<Leaf>()), Times.Never);
            postprocShouldNotExecuteBecauseWrongProcessingType.Verify(aX => aX.DoPreProcessing(source, target), Times.Never);
        }
    }
}
