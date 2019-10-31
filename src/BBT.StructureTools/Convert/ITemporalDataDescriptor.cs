// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Convert
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines a type which abstracts the retrieval of temporal data
    /// within the structure tools.
    /// </summary>
    /// <typeparam name="T">
    /// the type which is described.
    /// </typeparam>
    public interface ITemporalDataDescriptor<in T>
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
    }
}
