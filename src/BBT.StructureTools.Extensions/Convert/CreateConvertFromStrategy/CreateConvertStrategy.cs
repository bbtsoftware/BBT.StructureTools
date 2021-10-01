namespace BBT.StructureTools.Extensions.Convert
{
    using System.Collections.Generic;
    using BBT.StrategyPattern;
    using BBT.StructureTools;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Extension;

    /// <inheritdoc/>
    public class CreateConvertStrategy<TSource, TConcreteSource, TTarget, TConcreteTarget, TConcreteTargetImpl, TIntention>
        : ICreateConvertStrategy<TSource, TTarget, TIntention>
        where TSource : class
        where TConcreteSource : class, TSource
        where TTarget : class
        where TConcreteTarget : class, TTarget
        where TConcreteTargetImpl : class, TConcreteTarget, new()
        where TIntention : IBaseConvertIntention
    {
        private readonly IInstanceCreator<TConcreteTarget, TConcreteTargetImpl> creator;
        private readonly IConvert<TConcreteSource, TConcreteTarget, TIntention> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConvertStrategy{TSource, TConcreteSource, TTarget, TConcreteTarget, TConcreteTargetImpl, TIntention}"/> class.
        /// </summary>
        public CreateConvertStrategy(
            IInstanceCreator<TConcreteTarget, TConcreteTargetImpl> creator,
            IConvert<TConcreteSource, TConcreteTarget, TIntention> converter)
        {
            creator.NotNull(nameof(creator));
            converter.NotNull(nameof(converter));

            this.creator = creator;
            this.converter = converter;
        }

        /// <inheritdoc/>
        public TTarget CreateTarget(TSource source)
        {
            source.NotNull(nameof(source));

            var target = this.creator.Create();
            return target;
        }

        /// <inheritdoc/>
        public void Convert(TSource source, TTarget target, ICollection<IBaseAdditionalProcessing> additionalProcessings)
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));
            additionalProcessings.NotNull(nameof(additionalProcessings));

            this.converter.Convert((TConcreteSource)source, (TConcreteTarget)target, additionalProcessings);
        }

        /// <inheritdoc/>
        public bool IsResponsible(TSource criterion)
        {
            criterion.NotNull(nameof(criterion));
            return criterion is TConcreteSource;
        }
    }
}