namespace BBT.StructureTools.Extensions.Convert
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateTargetImplConvertTargetImplHelper<TSource, TTarget, TTargetImpl, TConvertIntention>
        : ICreateTargetImplConvertTargetImplHelper<TSource, TTarget, TTargetImpl, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TTargetImpl : class, TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TTargetImpl, TTargetImpl> instanceCreator;
        private readonly IConvert<TSource, TTargetImpl, TConvertIntention> convert;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTargetImplConvertTargetImplHelper{TSource,TTarget,TTargetImpl,TConvertIntention}" /> class.
        /// </summary>
        public CreateTargetImplConvertTargetImplHelper(
            IInstanceCreator<TTargetImpl, TTargetImpl> instanceCreator,
            IConvert<TSource, TTargetImpl, TConvertIntention> convert)
        {
            instanceCreator.NotNull(nameof(instanceCreator));
            convert.NotNull(nameof(convert));

            this.instanceCreator = instanceCreator;
            this.convert = convert;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(
            TSource source,
            ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            var target = this.instanceCreator.Create();
            this.convert.Convert(source, target, additionalProcessings);

            return target;
        }
    }
}