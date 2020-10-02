namespace BBT.StructureTools.Tests.Convert.TestData
{
    using System;
    using BBT.StructureTools.Extension;
    using BBT.StructureTools.Provider;

    /// <summary>
    /// Implementation of temporal data handler for test purposes.
    /// </summary>
    /// <typeparam name="T">Type of temporal data.</typeparam>
    public class TemporalDataHandler<T> : ITemporalDataHandler<T>
        where T : class, ITemporalData
    {
        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.GetBegin"/>.
        /// </summary>
        public DateTime GetBegin(T aData)
        {
            aData.NotNull(nameof(aData));

            return aData.From;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.GetEnd"/>.
        /// </summary>
        public DateTime GetEnd(T aData)
        {
            aData.NotNull(nameof(aData));

            return aData.To;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.IsReferenceDateWithin"/>.
        /// </summary>
        public bool IsReferenceDateWithin(
            T aData, DateTime aReferenceDate)
        {
            var isWithin = aData.From <= aReferenceDate && aData.To >= aReferenceDate;
            return isWithin;
        }

        /// <summary>
        /// See <see cref="ITemporalDataHandler{T}.SetEndInfinte"/>.
        /// </summary>
        public void SetEndInfinte(
            T aData, DateTime aInfiniteDate)
        {
            aData.To = new DateTime(9999, 12, 31);
        }
    }
}
