// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert.Strategy;
    using BBT.StructureTools.Copy;

    /// <summary>
    /// Helper for the convert operation.
    /// </summary>
    /// <typeparam name="TSource">The source to copy from.</typeparam>
    /// <typeparam name="TTarget">The target to copy to.</typeparam>
    public interface IConvertRegistration<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Registers a specific target attribute to copy <typeparamref name="TSource"/>
        /// into target.
        /// </summary>
        IConvertRegistration<TSource, TTarget> RegisterCopySource(
            Expression<Func<TTarget, TSource>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/>
        /// Source value is only copied if source value not equal to its default.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfSourceNotDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/>
        /// Source value is only copied if target value not equal to its default.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeIfTargetIsDefault<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttribute<TValue>(
            Func<TSource, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the source attribute to copy.</typeparam>
        /// <typeparam name="TTargetValue">The type of the target attribute to copy to.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithMapping<TSourceValue, TTargetValue>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> (or <paramref name="sourceLookUpFunc"/> if default)
        /// into value specified by <paramref name="targetExpression"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithLookUp<TValue>(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceLookUpFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/> with respect of <paramref name="sourceUpperLimitFunc"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithUpperLimit<TValue>(
            Func<TSource, TValue> sourceFunc,
            Func<TSource, TValue> sourceUpperLimitFunc,
            Expression<Func<TTarget, TValue>> targetExpression)
            where TValue : IComparable<TValue>;

        /// <summary>
        /// Registers a specific attribute to copy the value specified by
        /// <paramref name="sourceFunc"/> into value specified by
        /// <paramref name="targetExpression"/>. The source value can be filtered with
        /// <paramref name="sourceFunc"/> using filter arguments belonging to target.
        /// </summary>
        /// <typeparam name="TValue">See link above.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttributeWithSourceFilter<TValue>(
            Func<TSource, TTarget, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

        /// <summary>
        /// Registers the conversion of the derived type <typeparamref name="TSourceValue"/> of
        /// <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The source to convert from. Must derive from <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterSubConvert<TSourceValue, TConvertIntention>()
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers the conversion of the derived type <typeparamref name="TSourceValue"/> of
        /// <typeparamref name="TSource"/> to the derviced type <typeparamref name="TTargetValue"/> of
        /// <typeparamref name="TTarget"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The source to convert from. Must derive from <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The target to convert to. Must derive from <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterSubConvert<TSourceValue, TTargetValue, TConvertIntention>()
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers the conversion to the derived type <typeparamref name="TTargetValue"/> of
        /// <typeparamref name="TTarget"/>.
        /// </summary>
        /// <typeparam name="TTargetValue">The target to convert to. Must derive from <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterTargetSubConvert<TTargetValue, TConvertIntention>()
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a conversion from a ist of source values to specific target values filtered
        /// by <paramref name="filterFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">
        /// The type of the ist entries which shall be converted into
        /// the <typeparamref name="TTargetValue"/>s.</typeparam>
        /// <typeparam name="TTargetValue">
        /// The ist entries which shall be converted from
        /// the <typeparamref name="TSourceValue"/>s.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterConvertToMany<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TTarget, IEnumerable<TTargetValue>> targetFunc,
            Func<TSourceValue, TTargetValue, bool> filterFunc)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a conversion between different hierarchy levels.
        /// </summary>
        /// <typeparam name="TSourceValue">The source to convert from.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterConvertFromTargetOnDifferentLevels<TSourceValue, TConvertIntention>(
            Func<TTarget, TSourceValue> sourceExpression)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a conversion between different hierarchy levels.
        /// If <typeparamref name="TSourceValue"/> happens to be null (0..1 relations), the convert will not be processesed.
        /// </summary>
        /// <typeparam name="TSourceValue">The source to convert from.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterConvertFromSourceOnDifferentLevels<TSourceValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a conversion between different hierarchy levels.
        /// If <typeparamref name="TSourceValue"/> happens to be null (0..1 relations), the convert will not be processesed.
        /// </summary>
        /// <typeparam name="TSourceValue">The source to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The target to convert from (must derive from <typeparamref name="TTarget"/>).</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterConvertFromSourceOnDifferentLevels<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateFromSourceWithReverseRelation<TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Enables to register a <see cref="ISourceConvertStrategy{TBaseSource, TBaseTarget, TIntention}"/> strategy for converting models of different types
        /// for <c>to one</c> relationships.
        /// E.g.: This can be used to convert annuity / capital covers.
        /// </summary>
        /// <typeparam name="TBaseSource">Source base type (e.g. LiBaseCover).</typeparam>
        /// <typeparam name="TBaseTarget">Target base type (e.g. LiClaicover).</typeparam>
        /// <typeparam name="TIntention">Intention defining the conversion use case.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, TBaseSource> baseSourceFunc,
            Expression<Func<TTarget, TBaseTarget>> targetValueExpression,
            Expression<Func<TBaseTarget, TTarget>> targetParentExpression)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The property type on <typeparamref name="TSource"/> to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The property type on <typeparamref name="TSource"/> to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOne<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>from many</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyFromMany<TSourceValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers the copy of <typeparamref name="TSubCopy"/> from the source to the target.
        /// </summary>
        /// <typeparam name="TSubCopy">The <see cref="ICopy{TValue}"/> type of the value to copy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterSubCopy<TSubCopy>()
            where TSubCopy : class, ICopy<TSource>, ICopy<TTarget>;

        /// <summary>
        /// Registers a <c>to many</c> relationships where one hierarchy level between
        /// source and target is skipped.
        /// </summary>
        /// <typeparam name="TSourceValue">see link above.</typeparam>
        /// <typeparam name="TTargetValue">Also see link above.</typeparam>
        /// <typeparam name="TConcreteTargetValue">Again see link above.</typeparam>
        /// <typeparam name="TMergeValue">Once again see link above.</typeparam>
        /// <typeparam name="TConvertIntention">And yet see link above.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterMergeLevel<TSourceValue, TTargetValue, TConcreteTargetValue, TMergeValue, TConvertIntention>(
            Func<TSource, IEnumerable<TMergeValue>> mergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TMergeValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the ist entries on <typeparamref name="TTarget"/>.
        /// The <typeparamref name="TTarget"/> type and the <typeparamref name="TTargetValue"/> value
        /// must be of type <see cref="ICollection{TChildType}"/>.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on a target corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the ist entries on <typeparamref name="TTarget"/>.
        /// The <typeparamref name="TTarget"/> type and the <typeparamref name="TTargetValue"/> value
        /// must be of type <see cref="ICollection{TChildType}"/>.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the ist entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyGenericWithReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// Note, that the source (a collection type) cannot be null. The collection must
        /// be an instance but can be an empty collection.
        /// Note, that the target type <typeparamref name="TTarget"/> cannot be of type
        /// <see cref="ICollection{TChildType}"/>.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the ist entries on <typeparamref name="TTarget"/>.
        /// The <typeparamref name="TTarget"/> type and the <typeparamref name="TTargetValue"/> value
        /// cannot cannot be of type <see cref="ICollection{TChildType}"/>.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyGeneric<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, IEnumerable<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the ist entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the ist entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConcreteTargetValue">The concrete implementation of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithSourceFilterAndReverseRelation<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConcreteTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConcreteTargetValue : TTargetValue, new()
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers post processing operations which are passed to the converter
        /// while converting an object and then executed at the end of the convert process.
        /// </summary>
        IConvertRegistration<TSource, TTarget> RegisterPostProcessings(
            IConvertPostProcessing<TSource, TTarget> additionalProcessing, params IConvertPostProcessing<TSource, TTarget>[] furtherAdditionalProcessings);

        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        IConvertOperations<TSource, TTarget> EndRegistrations();

        /// <summary>
        /// Enables to register a <see cref="ISourceConvertStrategy{TBaseSource, TBaseTarget, TIntention}"/> strategy for converting models of different types
        /// for <c>to many</c> relationships.
        /// </summary>
        /// <typeparam name="TBaseSource">Source base type.</typeparam>
        /// <typeparam name="TBaseTarget">Target base type.</typeparam>
        /// <typeparam name="TIntention">Conversion intention which shall be used within the strategy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, IEnumerable<TBaseSource>> source, Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent, Expression<Func<TBaseTarget, TTarget>> reverseRelation)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>from temporal data</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the temporal collection entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyFromTemporalData<TSourceValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
            where TSourceValue : class
            where TConvertIntention : IBaseConvertIntention;
    }
}
