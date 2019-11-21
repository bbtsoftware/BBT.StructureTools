namespace BBT.StructureTools.Copy.Strategy
{
    using BBT.StrategyPattern;

    /// <summary>
    /// Generic base interface for a strategy which copies a source object to a target object. Intended
    /// to be used with abstract base classes.
    /// </summary>
    /// <typeparam name="T">Type to copy.</typeparam>
    public interface ICopyStrategy<T> : IGenericStrategy<T>
    {
        /// <summary>
        /// Creates an instance of a class inheriting from T.
        /// </summary>
        T Create();

        /// <summary>
        /// Copies the <see paramref="source"/> to the <see paramref="target"/>.
        /// </summary>
        void Copy(T source, T target, ICopyCallContext copyCallContext);
    }
}