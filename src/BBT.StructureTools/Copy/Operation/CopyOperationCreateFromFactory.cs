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
        private Expression<Func<T, TValue>> mTargetExpression;

        private Func<TAttributeValueFactory, TValue> mAttrValueFunc;

        private TAttributeValueFactory mAttributeValueFactory;

        /// <summary>
        /// <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var lValue = this.mAttrValueFunc.Invoke(this.mAttributeValueFactory);
            target.SetPropertyValue(this.mTargetExpression, lValue);
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

            var lAttributeValueFactory = IocHandler.Instance.IocResolver.GetInstance<TAttributeValueFactory>();
            lAttributeValueFactory.Should().NotBeNull();

            this.mAttributeValueFactory = lAttributeValueFactory;
            this.mAttrValueFunc = aAttrValueExpression.Compile();
            this.mTargetExpression = targetExpression;
        }
    }
}