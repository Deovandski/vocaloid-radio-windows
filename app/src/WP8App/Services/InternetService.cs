using Microsoft.Phone.Net.NetworkInformation;
using WPAppStudio.Services.Interfaces;

namespace WPAppStudio.Services
{
	/// <summary>
    /// Implementation of a internet service.
    /// </summary>
    public class InternetService : IInternetService
    {
		/// <summary>
        /// Check if is network avalaible.
        /// </summary>
        public bool IsNetworkAvailable()
        {
//#if DEBUG
//            return false;
//#else
            return NetworkInterface.GetIsNetworkAvailable();
//#endif
        }
    }
}
