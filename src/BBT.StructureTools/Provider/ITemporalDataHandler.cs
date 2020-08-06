namespace BBT.StructureTools.Provider
{
    using System;
    using BBT.StructureTools.Constants;

    /// <summary>
    /// Defines a type which abstracts the handling of temporal data
    /// within the structure tools.
    /// </summary>
    /// <typeparam name="T">
    /// the type which is wrapped.
    /// </typeparam>
    public interface ITemporalDataHandler<in T>
        where T : class
    {
        /// <summary>
        /// Gets the <see cref="DateTime"/> which marks the begin
        /// of the temporal period defined by <paramref name="data"/>.
        /// </summary>
        DateTime GetBegin(T data);

        /// <summary>
        /// Gets the <see cref="DateTime"/> which marks the end
        /// of the temporal period defined by <paramref name="data"/>.
        /// </summary>
        DateTime GetEnd(T data);

        /// <summary>
        /// Returns true if the <paramref name="referenceDate"/> is within the <see cref="GetEnd(T)"/> and <see cref="GetBegin(T)"/>
        /// range. Begin and end themselfes are part of the range.
        /// </summary>
        bool IsReferenceDateWithin(T data, System.DateTime referenceDate);

        /// <summary>
        /// Sets the end of <paramref name="data"/> to the <see cref="TemporalConstants.InfiniteDate"/>.
        /// </summary>
        void SetEndInfinte(T data, DateTime infiniteDate);
    }
}
