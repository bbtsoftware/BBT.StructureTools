namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy.Operation;
    using BBT.StructureTools.Copy.Strategy;

    /// <summary>
    /// Helper for the copy mechanism.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    public interface ICopyHelperRegistration<T>
        where T : class
    {
        /// <summary>
        /// Register a copy attribute of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute to copy. Must be a reference type.</typeparam>
        ICopyHelperRegistration<T> RegisterAttribute<TValue>(Expression<Func<T, TValue>> expression);

        /// <summary>
        /// Registers an attribute to retrieve it's value from a factory during the copy process.
        /// </summary>
        /// <typeparam name="TAttributeValueFactory">Interface type of the Attribute value factory.</typeparam>
        /// <typeparam name="TValue">Type of the attribute to copy. must be a reference type.</typeparam>
        ICopyHelperRegistration<T> RegisterCreateFromFactory<TAttributeValueFactory, TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TAttributeValueFactory, TValue>> attrValueExpression)
            where TAttributeValueFactory : class;

            /// <summary>
        /// Registers an attribute to retrieve it's value from a factory during the copy process,
        /// based on a struct.
        /// </summary>
        /// <typeparam name="TValue">Type of the attribute to copy. must be a reference type.</typeparam>
        ICopyHelperRegistration<T> RegisterInlineValueProcessing<TValue>(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TValue>> attrValueExpression);

        /// <summary>
        /// Registers a <see cref="IEnumerable{T}"/> on a source to be copied into a ICollection{TChildType}
        /// where each Child has a reverse relation onto the parent.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChildType">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategy<TStrategy, TChildType>(
            Func<T, IEnumerable<TChildType>> sourceFunc,
            Expression<Func<T, ICollection<TChildType>>> targetExpression,
            Expression<Func<TStrategy, TChildType>> createTargetChildExpression)
            where TStrategy : class, ICopyStrategy<TChildType>
            where TChildType : class;

        /// <summary>
        /// Registers a <see cref="IEnumerable{T}"/> on a source to be copied into a ICollection{TChildType}
        /// where each Child has a reverse relation onto the parent.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<T, ICollection<TChild>>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class;

        /// <summary>
        /// Registers a create-copy operation on a source for a <see cref="IEnumerable{TChild}"/> of (child-)elements with a reference to the source
        /// The source does not have a collection of childs.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToManyFromGenericStrategyReverseRelationOnly<TStrategy, TChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class;

        /// <summary>
        /// Registers a <see cref="IEnumerable{T}"/> on a source to be copied into a ICollection{TChildType}
        /// where each Child has a reverse relation onto the parent.
        /// </summary>
        /// <typeparam name="TStrategy">
        /// Strategy defining type.
        /// </typeparam>
        /// <typeparam name="TChild">
        /// Definition of the type defined in the strategy.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToOneFromGenericStrategyWithReverseRelation<TStrategy, TChild>(
            Expression<Func<T, TChild>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationExpression)
            where TStrategy : class, ICopyStrategy<TChild>
            where TChild : class;

        /// <summary>
        /// Register a sub copy for inherited attributes.
        /// </summary>
        /// <typeparam name="TSubCopyOfT">
        /// An ICopy of T.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterSubCopy<TSubCopyOfT>()
            where TSubCopyOfT : class, ICopy<T>;

        /// <summary>
        /// Registers a <see cref="IEnumerable{T}"/> on a source to be copied into a ICollection{TChildType}
        /// where each Child has a reverse relation onto the parent.
        /// </summary>
        /// <typeparam name="TChild">
        /// Type of the child - Either an interface or a class.
        /// </typeparam>
        /// <typeparam name="TConcreteChild">
        /// Concrete implementation of TChild. Must be a class.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelation<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<T, ICollection<TChild>>> targetExpression,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new();

        /// <summary>
        /// Registers a <see cref="IEnumerable{T}"/> on a source to be copied
        /// where each Child has a reverse relation onto the parent only.
        /// </summary>
        /// <typeparam name="TChild">
        /// Type of the child - Either an interface or a class.
        /// </typeparam>
        /// <typeparam name="TConcreteChild">
        /// Concrete implementation of TChild. Must be a class.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterCreateToManyWithReverseRelationOnly<TChild, TConcreteChild>(
            Func<T, IEnumerable<TChild>> sourceFunc,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new();

        /// <summary>
        /// Registers a parent - child relation where the child object has a reverse relational
        /// reference to the parent.
        /// </summary>
        /// <typeparam name="TChild">
        /// Either interface or class type. Defines the childs type.
        /// </typeparam>
        /// <typeparam name="TConcreteChild">
        /// A concrete implementation of the TChild parameter, must be convert-able to TCHILD.
        /// </typeparam>
        /// <param name="targetFuncExpr">
        ///     The a Target Function Expression.
        /// </param>
        /// <param name="reverseRelationFunc">
        ///     expression describing the reverse relation from the child to the parent.
        /// </param>
        /// <returns>
        /// this object.
        /// </returns>
        ICopyHelperRegistration<T> RegisterCreateToOneWithReverseRelation<TChild, TConcreteChild>(
            Expression<Func<T, TChild>> targetFuncExpr,
            Expression<Func<TChild, T>> reverseRelationFunc)
            where TChild : class
            where TConcreteChild : class, TChild, new();

        /// <summary>
        /// Registers post processing operations which are passed to the <see cref="ICopy{TClassToCopy}"/>
        /// while copying an object and then executed at the end of the copy process.
        /// </summary>
        ICopyHelperRegistration<T> RegisterPostProcessings(
            IBaseAdditionalProcessing additionalProcessing, params IBaseAdditionalProcessing[] furtherAdditionalProcessings);

        /// <summary>
        /// Registers post processing operations which are passed to the <see cref="ICopy{TClassToCopy}"/>
        /// The post processing is resolved via IoC container.
        /// </summary>
        /// <typeparam name="TPostProcessing">
        /// post processing type to resolve from IoC container.
        /// </typeparam>
        ICopyHelperRegistration<T> RegisterPostProcessing<TPostProcessing>()
            where TPostProcessing : IBaseAdditionalProcessing;

        /// <summary>
        /// Registers cross reference processing while copying.
        /// </summary>
        /// <typeparam name="TCrossReferencedModel">The type of the cross referenced model.</typeparam>
        /// <typeparam name="TReferencingModel">The typeof the referencing model.</typeparam>
        ICopyHelperRegistration<T> RegisterCrossReferenceProcessing<TCrossReferencedModel, TReferencingModel>(
            Expression<Func<TReferencingModel, TCrossReferencedModel>> referencingProperty)
            where TCrossReferencedModel : class
            where TReferencingModel : class;

        /// <summary>
        /// Ends the registrations and start the operation phase.
        /// </summary>
        ICopyOperation<T> EndRegistrations();
    }
}