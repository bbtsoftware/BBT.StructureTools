namespace BBT.StructureTools.Extension
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension class for collections.
    /// </summary>
    internal static class CollectionExtension
    {
        /// <summary>
        /// Add the <paramref name="collectionToBeAdded"/> items to <paramref name="collectionToAddTo"/>.
        /// </summary>
        /// <typeparam name="TBase">Type of the collection to which elements of <typeparamref name="TChild"/> added.</typeparam>
        /// <typeparam name="TChild">Type of the collection elements which are added to the <paramref name="collectionToAddTo"/>.</typeparam>
        public static void AddRangeToMe<TBase, TChild>(this ICollection<TBase> collectionToAddTo, IEnumerable<TChild> collectionToBeAdded)
            where TChild : TBase
        {
            foreach (var item in collectionToBeAdded)
            {
                collectionToAddTo.Add(item);
            }
        }
    }
}
