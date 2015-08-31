
namespace WPAppStudio.Services.Interfaces
{
    /// <summary>
    /// Interface for the Proximity service.
    /// </summary>
    public interface IProximityService
    {
        /// <summary>
        /// NFC is available
        /// </summary>
        bool IsProximityAvailable { get; }

		/// <summary>
        /// Publish a uri message using the Proximity service.
        /// </summary>
        /// <param name="uriString">The uri shared.</param>
        void ShareUri(string uriString);
    }
}
