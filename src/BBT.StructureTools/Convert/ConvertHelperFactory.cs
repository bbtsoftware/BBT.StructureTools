// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TSource">See link above.</typeparam>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    /// <typeparam name="TConvertIntention">See link above.</typeparam>
    public class ConvertHelperFactory<TSource, TTarget, TConcreteTarget, TConvertIntention>
        : IConvertHelperFactory<TSource, TTarget, TConcreteTarget, TConvertIntention>
        where TSource : class
        where TTarget : class
        where TConcreteTarget : TTarget, new()
        where TConvertIntention : IBaseConvertIntention
    {
        /// <summary>
        /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}.GetConvertHelper{TReverseRelation}"/>.
        /// </summary>
        /// <typeparam name="TReverseRelation">See link above.</typeparam>
        public ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention> GetConvertHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> aReverseRelationFunc)
            where TReverseRelation : class
        {
            aReverseRelationFunc.Should().NotBeNull();

            var lConvertHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TReverseRelation, TConvertIntention>>();
            lConvertHelper.SetupReverseRelation(aReverseRelationFunc);
            return lConvertHelper;
        }

        /// <summary>
        /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}.GetConvertHelper"/>.
        /// </summary>
        public ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention> GetConvertHelper()
        {
            var lConvertHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateConvertHelper<TSource, TTarget, TConcreteTarget, TConvertIntention>>();
            return lConvertHelper;
        }
    }
}
