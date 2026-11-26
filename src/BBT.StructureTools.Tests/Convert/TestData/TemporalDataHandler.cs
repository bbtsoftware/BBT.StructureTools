namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <summary>
    /// Implementation of temporal data handler for test purposes.
    /// </summary>
    /// <typeparam name="T">Type of temporal data.</typeparam>
    internal class TemporalDataHandler<T> : ITemporalDataHandler<T>
        where T : class, ITemporalData
    {
        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.GetBegin"/>.
        /// </summary>
        public DateTime GetBegin(T data)
        {
            data.NotNull(nameof(data));

            return data.From;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.GetEnd"/>.
        /// </summary>
        public DateTime GetEnd(T data)
        {
            data.NotNull(nameof(data));

            return data.To;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.IsReferenceDateWithin"/>.
        /// </summary>
        public bool IsReferenceDateWithin(
            T data, DateTime referenceDate)
        {
            var isWithin = data.From <= referenceDate && data.To >= referenceDate;
            return isWithin;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.SetEndInfinte"/>.
        /// </summary>
        public void SetEndInfinte(
            T data, DateTime infiniteDate)
        {
            data.To = new DateTime(9999, 12, 31);
        }
    }
}
