namespace BBT.StructureTools.Copy
{
    using BBT.StructureTools.Copy.Helper;
    using BBT.StructureTools.Copy.Marker;

    /// <summary>
    /// Used to declare properties to copy.
    /// </summary>
    /// <typeparam name="T">class to copy.</typeparam>
    public interface ICopyRegistrations<T> : ICopyRegistrationMarker
        where T : class
    {
        /// <summary>
        /// Used to declare properties to copy.
        /// </summary>
        void DoRegistrations(ICopyHelperRegistration<T> registrations);
    }
}