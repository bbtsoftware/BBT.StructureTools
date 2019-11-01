namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <inheritdoc/>
    public class CopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory> : ICopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>
        where T : class
        where TAttributeValueFactory : class
    {
        private Expression<Func<T, TValue>> targetexpression;

        private Func<TAttributeValueFactory, TValue> attrValueFunc;

        private TAttributeValueFactory attributeValueFactory;

        /// <inheritdoc/>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var value = this.attrValueFunc.Invoke(this.attributeValueFactory);
            target.SetPropertyValue(this.targetexpression, value);
        }

        /// <inheritdoc/>
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