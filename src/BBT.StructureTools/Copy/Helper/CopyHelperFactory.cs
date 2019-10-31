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
            Expression<Func<TTarget, TReverseRelation>> reverseRelationFunc)
            where TReverseRelation : class
        {
            reverseRelationFunc.Should().NotBeNull();

            var copyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget, TReverseRelation>>();
            copyHelper.SetupReverseRelation(reverseRelationFunc);
            return copyHelper;
        }

        /// <summary>
        /// See <see cref="IConvertHelperFactory{TSource, TTarget, TConcreteTarget, TConvertIntention}.GetConvertHelper"/>.
        /// </summary>
        public ICreateCopyHelper<TTarget, TConcreteTarget> GetCopyHelper()
        {
            var copyHelper = IocHandler.Instance.IocResolver.GetInstance<ICreateCopyHelper<TTarget, TConcreteTarget>>();
            return copyHelper;
        }
    }
}
