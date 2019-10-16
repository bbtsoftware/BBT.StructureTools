namespace BBT.StructureTools.Initialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface which defines how instances of a service are being retrieved from
    /// an IoC container within the BBT.StructureTools.
    /// </summary>
    public interface IIocResolver
    {
        /// <summary>
        /// Returns an instance of TService. ResolutionFailedException: If there is no
        /// corresponding service is registered, if more than one service is registered
        /// or if there is a problem with the resolving of the service (e.g. missing dependency).
        /// or exception when instantiating service or dependency).
        /// </summary>
        /// <typeparam name="TService">The service to resolve.</typeparam>
        /// <returns>The resolved object.</returns>
        TService GetInstance<TService>();

        /// <summary>
        /// Returns all instances of TService.If no instances are registered
        /// an empty ist is returned.Derivatives are not returned.
        /// ResolutionFailedException: If a problem occurs during resolving from service
        /// (e.g.missing dependency or exception when instantiating service or dependency).
        /// </summary>
        /// <typeparam name="TService">The service to resolve.</typeparam>
        /// <returns>The resolved object.</returns>
        IEnumerable<TService> GetAllInstances<TService>();

        /// <summary>
        /// Returns an instance of aServiceType.ResolutionFailedException: If there is no
        /// The corresponding service is registered if several services are registered.
        /// or if there is a problem with the resolver of the service (for example, missing dependency
        /// or Exception when instantiating Service or Dependency).
        /// </summary>
        object GetInstance(Type serviceType);
    }
}
