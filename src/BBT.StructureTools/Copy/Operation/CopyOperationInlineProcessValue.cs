// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Operation
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Extension;
    using FluentAssertions;

    /// <summary>
    /// Defines a strategy to handle inline processing of values on copy targets.
    /// </summary>
    /// <typeparam name="T">type as T.</typeparam>
    /// <typeparam name="TValue">type of the value on type T.</typeparam>
    public class CopyOperationInlineProcessValue<T, TValue> : ICopyOperation<T>
        where T : class
    {
        private readonly Expression<Func<T, TValue>> targetexpression;

        private readonly Func<TValue> valueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyOperationInlineProcessValue{T,TValue}"/> class.
        /// </summary>
        public CopyOperationInlineProcessValue(
            Expression<Func<T, TValue>> targetExpression,
            Expression<Func<TValue>> attrValueExpression)
        {
            targetExpression.Should().NotBeNull();
            attrValueExpression.Should().NotBeNull();

            this.targetexpression = targetExpression;
            this.valueFunc = attrValueExpression.Compile();
        }

        /// <summary>
        /// See <see cref="ICopyOperation{T}.Copy"/>.
        /// </summary>
        public void Copy(T source, T target, ICopyCallContext copyCallContext)
        {
            target.SetPropertyValue(this.targetexpression, this.valueFunc.Invoke());
        }
    }
}