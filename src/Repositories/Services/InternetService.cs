using System;
using Microsoft.Phone.Net.NetworkInformation;
using WPAppStudio.Services.Interfaces;

namespace WPAppStudio.Services
{
	/// <summary>
    /// Implementation of a internet service.
    /// </summary>
    public class InternetService : IInternetService
    {
	    public InternetService()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += DeviceNetworkInformation_NetworkAvailabilityChanged;
        }

		public event EventHandler InternetAvailabilityChanged;
				
        void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            var handlerTmp = InternetAvailabilityChanged;

            if (handlerTmp != null)
                handlerTmp(this, new EventArgs());
        }
		
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
