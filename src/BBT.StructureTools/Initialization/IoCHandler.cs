// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Initialization
{
    using System;

    /// <summary>
    /// The IoC handler as singelton.
    /// </summary>
    public sealed class IocHandler
    {
        private static readonly Lazy<IocHandler> Lazy = new Lazy<IocHandler>(() => new IocHandler());

        /// <summary>
        /// Initializes a new instance of the <see cref="IocHandler"/> class.
        /// Singelton pattern.
        /// </summary>
        private IocHandler()
        {
        }

        /// <summary>
        /// Gets singleton pattern.
        /// </summary>
        public static IocHandler Instance
        {
            get { return Lazy.Value; }
        }

        /// <summary>
        /// Gets or sets the object to be set for IoC handling.
        /// </summary>
        public IIocResolver IocResolver { get; set; }
    }
}
