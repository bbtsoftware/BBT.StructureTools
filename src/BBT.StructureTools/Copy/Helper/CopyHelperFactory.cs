// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Helper
{
    using System;
    using System.Linq.Expressions;
    using BBT.StructureTools.Convert;
    using BBT.StructureTools.Copy;
    using BBT.StructureTools.Initialization;
    using FluentAssertions;

    /// <summary>
    /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}"/>.
    /// </summary>
    /// <typeparam name="TTarget">See link above.</typeparam>
    /// <typeparam name="TConcreteTarget">See link above.</typeparam>
    public class CopyHelperFactory<TTarget, TConcreteTarget>
        : ICopyHelperFactory<TTarget, TConcreteTarget>
        where TTarget : class
        where TConcreteTarget : class, TTarget, new()
    {
        /// <summary>
        /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}.GetConvertHelper{TReverseRelation}"/>.
        /// </summary>
        /// <typeparam name="TReverseRelation">See link above.</typeparam>
        public ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation> GetCopyHelper<TReverseRelation>(
            Expression<Func<TTarget, TReverseRelation>> aReverseRelationFunc)
            where TReverseRelation : class
        {
            aReverseRelationFunc.Should().NotBeNull();

            var lCopyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation>>();
            lCopyHelper.SetupReverseRelation(aReverseRelationFunc);
            return lCopyHelper;
        }

        /// <summary>
        /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}.GetConvertHelper"/>.
        /// </summary>
        public ICreateCopyHelper<TTarget, TConcreteTarget> GetCopyHelper()
        {
            var lCopyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget>>();
            return lCopyHelper;
        }
    }
}
