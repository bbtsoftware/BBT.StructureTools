// Copyright © BBT Software AG. All rights reserved.

namespace BBT.StructureTools.Copy.Marker
{
    /// <summary>
    /// Defines a marker interface which shall be used to mark implementations of
    /// <see cref="ICopyRegistrations{T}"/> in order to identify all those implementations by their
    /// type information regardless of their generic parameters.
    /// </summary>
    public interface ICopyRegistrationMarker
    {
    }
}