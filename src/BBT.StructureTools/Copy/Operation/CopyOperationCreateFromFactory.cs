namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Initialization;

    /// <summary>
    /// <see cref="ICopyOperationCreateFromFactory{T,TValue,TAttributeValueFactory}"/>.
    /// </summary>
    /// <typeparam name="T">see above.</typeparam>
    /// <typeparam name="TValue">see above.</typeparam>
    /// <typeparam name="TAttributeValueFactory">see above.</typeparam>
    internal class CopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory> : ICopyOperationCreateFromFactory<T, TValue, TAttributeValueFactory>
        where T : class
        where TAttributeValueFactory : class
    {
        private Expression<Func<T, TValue>> targetExpression;

        private Func<TAttributeValueFactory, TValue> attrValueFunc;

        private TAttributeValueFactory attributeValueFactory;

        /// <summary>
        /// <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            var value = this.attrValueFunc.Invoke(this.attributeValueFactory);
            target.SetPropertyValue(this.targetExpression, value);
        }

        /// <summary>
        /// <see cref="ICopyOperationCreateFromFactory{T,TValue,TAttributeValueFactory}.Initialize"/>.
        /// </summary>
        /// <param name="targetExpression">see above.</param>
        /// <param name="attrValueExpression">see above.</param>
        public void Initialize(Expression<Func<T, TValue>> targetExpression, Expression<Func<TAttributeValueFactory, TValue>> attrValueExpression)
        {
            targetExpression.NotNull(nameof(targetExpression));
            attrValueExpression.NotNull(nameof(attrValueExpression));

            var attributeValueFactory = IocHandler.Instance.IocResolver.GetInstance<TAttributeValueFactory>();
            attributeValueFactory.NotNull(nameof(attributeValueFactory));

            this.attributeValueFactory = attributeValueFactory;
            this.attrValueFunc = attrValueExpression.Compile();
            this.targetExpression = targetExpression;
        }
    }
}