// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Extension
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension class for collections.
    /// </summary>
    internal static class CollectionExtension
    {
        /// <summary>
        /// Add the <paramref name="aCollectionToBeAdded"/> items to <paramref name="aCollectionToAddTo"/>.
        /// </summary>
        /// <typeparam name="TBase">Type of the collection to which elements of <typeparamref name="TChild"/> added.</typeparam>
        /// <typeparam name="TChild">Type of the collection elements which are added to the <paramref name="aCollectionToAddTo"/>.</typeparam>
        public static void AddRangeToMe<TBase, TChild>(this ICollection<TBase> aCollectionToAddTo, IEnumerable<TChild> aCollectionToBeAdded)
            where TChild : TBase
        {
            foreach (var item in aCollectionToBeAdded)
            {
                aCollectionToAddTo.Add(item);
            }
        }
    }
}
