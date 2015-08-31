using System;

namespace WPAppStudio.Services.Interfaces
{
	/// <summary>
    /// Interface for a Internet service.
    /// </summary>
    public interface IInternetService
    {
		event EventHandler InternetAvailabilityChanged;
	
        bool IsNetworkAvailable();
    }
}
