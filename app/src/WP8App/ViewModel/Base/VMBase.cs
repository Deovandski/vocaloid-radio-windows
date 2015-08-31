using System;
using System.Collections.Generic;
using System.Windows;
using WPAppStudio.Entities.Base;
using WPAppStudio.Services;
using WPAppStudio.Services.Interfaces;

namespace WPAppStudio.ViewModel.Base
{
    /// <summary>
    /// ViewModel base for viewmodel specific work
    /// </summary>
    public class VMBase : BindableBase
    {
        /// <summary>
        /// Indicates if the viewmodel is in busy state.
        /// </summary>
        //private bool _isBusy;

        /// <summary>
        /// Text to show on UX when we are in busy state.
        /// </summary>
        //private string _busyText;

        private IInternetService _internetService;

        /// <summary>
        /// Constructor
        /// </summary>
        public VMBase()
        {
            _internetService = (App.Current.Resources["ViewModelLocator"] as ViewModelLocatorService).ResolveService<IInternetService>();

            _internetService.InternetAvailabilityChanged += _internetService_InternetAvailabilityChanged;
        }

        /// <summary>
        /// Initialize method, called on resume activation from application class.
        /// </summary>
        /// <param name="parameters"></param>
        public override void Initialize(IDictionary<string, string> parameters)
        {
            base.Initialize(parameters);

            this.OnPropertyChanged("IsInternetAvailable");
        }

        private void _internetService_InternetAvailabilityChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged("IsInternetAvailable");
        }

        /// <summary>
        /// Changed every time internet connectivity changes.
        /// </summary>
        public Visibility IsInternetAvailable
        {
            get { return _internetService.IsNetworkAvailable() ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}
