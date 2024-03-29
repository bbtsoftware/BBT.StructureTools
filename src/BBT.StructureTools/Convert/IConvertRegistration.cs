﻿namespace BBT.StructureTools.Convert
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
        /// <paramref name="targetExpression"/>. The source value can be filtered with
        /// <paramref name="sourceFunc"/> using filter arguments belonging to target.
        /// </summary>
        /// <typeparam name="TValue">See link above.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyAttribute<TValue>(
            Func<TSource, TTarget, TValue> sourceFunc,
            Expression<Func<TTarget, TValue>> targetExpression);

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
        /// Registers a conversion from a list of source values to specific target values filtered
        /// by <paramref name="filterFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">
        /// The type of the list entries which shall be converted into
        /// the <typeparamref name="TTargetValue"/>s.</typeparam>
        /// <typeparam name="TTargetValue">
        /// The list entries which shall be converted from
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
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateFromSourceWithReverseRelation<TTargetValue, TConvertIntention>(
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSource, TTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Enables to register a <see cref="ISourceConvertStrategy{TBaseSource, TBaseTarget, TIntention}"/> strategy for converting models of different types
        /// for <c>to one</c> relationships.
        /// E.g.: This can be used to convert annuity / capital covers.
        /// </summary>
        /// <typeparam name="TBaseSource">Source base type (e.g. LiBaseCover).</typeparam>
        /// <typeparam name="TBaseTarget">Target base type (e.g. LiClaimCover).</typeparam>
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
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithReverseRelation<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The property type on <typeparamref name="TSource"/> to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TRelation">The type of relation of the created target value.
        /// This relation is set by <see cref="ICreateConvertHelper{TSourceValue, TTargetValue, TRelation, TConvertIntention}"/>
        /// after creation of <typeparamref name="TTargetValue"/> using its reference function.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithRelation<TSourceValue, TTargetValue, TRelation, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The property type on <typeparamref name="TSource"/> to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TRelation">The type of relation of the created target value.
        /// This relation is set by <see cref="ICreateConvertHelper{TSourceValue, TTargetValue, TRelation, TConvertIntention}"/>
        /// after creation of <typeparamref name="TTargetValue"/> using its reference function.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneWithRelation<TSourceValue, TTargetValue, TRelation, TConvertIntention>(
            Func<TSource, TTarget, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to one</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The property type on <typeparamref name="TSource"/> to convert from.</typeparam>
        /// <typeparam name="TTargetValue">The property type <typeparamref name="TTarget"/> to convert into.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOne<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, TSourceValue> sourceFunc,
            Expression<Func<TTarget, TTargetValue>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>from history</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the history collection entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTemporalDataType">Temporal data type for resolving the handler.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCopyFromHist<TSourceValue, TTemporalDataType, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Func<TSource, TTarget, DateTime> referenceDateFunc)
            where TSourceValue : class, TTemporalDataType
            where TTemporalDataType : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>from many</c> relationship on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
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
        /// <typeparam name="TMergeValue">Once again see link above.</typeparam>
        /// <typeparam name="TConvertIntention">And yet see link above.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterMergeLevel<TSourceValue, TTargetValue, TMergeValue, TConvertIntention>(
            Func<TSource, IEnumerable<TMergeValue>> mergeFunc,
            Func<TMergeValue, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TTarget, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TMergeValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the list entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithReverseRelation<TSourceValue, TTargetValue, TReverseRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TReverseRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the list entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TRelation">The type of relation of the created target value.
        /// This relation is set by <see cref="ICreateConvertHelper{TSourceValue, TTargetValue, TRelation, TConvertIntention}"/>
        /// after creation of <typeparamref name="TTargetValue"/> using its reference function.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithRelation<TSourceValue, TTargetValue, TRelation, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the list entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TRelation">The type of relation of the created target value.
        /// This relation is set by <see cref="ICreateConvertHelper{TSourceValue, TTargetValue, TRelation, TConvertIntention}"/>
        /// after creation of <typeparamref name="TTargetValue"/> using its reference function.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyWithRelation<TSourceValue, TTargetValue, TRelation, TConvertIntention>(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, TRelation> relationFunc,
            ICreateConvertHelper<TSourceValue, TTargetValue, TRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TRelation : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// Note, that the source (a collection type) cannot be null. The collection must
        /// be an instance but can be an empty collection.
        /// <typeparamref name="TSourceValue"/>s are filtered with <paramref name="sourceFunc"/>.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the list entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToMany<TSourceValue, TTargetValue, TConvertIntention>(
            Func<TSource, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            ICreateConvertHelper<TSourceValue, TTargetValue, TConvertIntention> createConvertHelper)
            where TSourceValue : class
            where TTargetValue : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Registers a <c>to many</c> relationships on source and corresponding target.
        /// </summary>
        /// <typeparam name="TSourceValue">The type of the list entries on <typeparamref name="TSource"/>.</typeparam>
        /// <typeparam name="TTargetValue">The type of the list entries on <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TReverseRelation">The reverse relation of the created target value.
        /// Must be a base type of <typeparamref name="TTarget"/>.</typeparam>
        /// <typeparam name="TTemporalData">Temporal data type for resolving the handler.</typeparam>
        /// <typeparam name="TConvertIntention">The intention of the conversion.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToOneHistWithCondition<TSourceValue, TTargetValue, TReverseRelation, TTemporalData, TConvertIntention>(
            Func<TSource, TTarget, IEnumerable<TSourceValue>> sourceFunc,
            Expression<Func<TTarget, ICollection<TTargetValue>>> targetExpression,
            Func<TSource, TTarget, bool> toOneHistCriteria,
            Func<TSource, TTarget, DateTime> toOneReferenceDate,
            ICreateConvertHelper<TSourceValue, TTargetValue, TReverseRelation, TConvertIntention> createConvertHelper)
            where TSourceValue : class, TTemporalData
            where TTargetValue : class, TTemporalData
            where TReverseRelation : class
            where TTemporalData : class
            where TConvertIntention : IBaseConvertIntention;

        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        IConvertOperations<TSource, TTarget> EndRegistrations();

        /// <summary>
        /// Enables to register a <see cref="ISourceConvertStrategy{TBaseSource, TBaseTarget, TIntention}"/> strategy for converting models of different types
        /// for <c>to many</c> relationships.
        /// E.g.: This can be used to convert annuity / capital cover templates.
        /// </summary>
        /// <typeparam name="TBaseSource">Source base type (e.g. LiBaseCoverTemplate).</typeparam>
        /// <typeparam name="TBaseTarget">Target base type (e.g. LiBaseCover).</typeparam>
        /// <typeparam name="TIntention">Conversion intention which shall be used within the strategy.</typeparam>
        IConvertRegistration<TSource, TTarget> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TBaseSource, TBaseTarget, TIntention>(
            Func<TSource, IEnumerable<TBaseSource>> source,
            Expression<Func<TTarget, ICollection<TBaseTarget>>> targetParent,
            Expression<Func<TBaseTarget, TTarget>> reverseRelation)
            where TBaseSource : class
            where TBaseTarget : class
            where TIntention : IBaseConvertIntention;
    }
}
