// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <summary>
    /// <see cref="ICopyOperationCreateFromFactory{T,TValue,TAttributeValueFactory}"/>.
    /// </summary>
    /// <typeparam name="T">see above.</typeparam>
    /// <typeparam name="TValue">see above.</typeparam>
    /// <typeparam name="TAttributeValueFactory">see above.</typeparam>
    public class CopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory> : ICopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>
        where T : class
        where TAttributeValueFactory : class
    {
        private Expression<Func<T, TValue>> targetexpression;

        private Func<TAttributeValueFactory, TValue> attrValueFunc;

        private TAttributeValueFactory attributeValueFactory;

        /// <summary>
        /// <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var value = this.attrValueFunc.Invoke(this.attributeValueFactory);
            target.SetPropertyValue(this.targetexpression, value);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateFromFactory{T,TValue,TAttributeValueFactory}.Initialize"/>.
        /// </summary>
        /// <param name="targetExpression">see above.</param>
        /// <param name="aAttrValueExpression">see above.</param>
        public void Initialize(Expression<Func<T, TValue>> targetExpression, Expression<Func<TAttributeValueFactory, TValue>> aAttrValueExpression)
        {
            targetExpression.Should().NotBeNull();
            aAttrValueExpression.Should().NotBeNull();

            var attributeValueFactory = IocHandler.Instance.IocResolver.GetInstance<TAttributeValueFactory>();
            attributeValueFactory.Should().NotBeNull();

            this.attributeValueFactory = attributeValueFactory;
            this.attrValueFunc = aAttrValueExpression.Compile();
            this.targetexpression = targetExpression;
        }
    }
}