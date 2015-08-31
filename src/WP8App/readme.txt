Your application may have been created using open source. Please double-check the source code of the application to determine which open source files are in your application and then comply with any license requirements. 
For more information on open source licenses used by Windows Phone App Studio, review the following:

	Windows Phone Toolkit         (http://phone.codeplex.com)   
	Microsoft Unity               (http://unity.codeplex.com)     
	MyToolkit                     (http://mytoolkit.codeplex.com)         


Instructions for setting PubCenter AdControl properties.
--------------------------------------------------------------
You can see sample code for the AdControl implementation in the main panorama page. The code is commented, so if you
want to use it, you need to uncomment it:


In the AdControl XAML you have two important properties: AdUnitId and ApplicationId. The current values are intended 
for demo pourposes and need to be replaced before publishing your App in the Windows Phone Store.

To obtain an AdUnitId and ApplicationId keys, you need to register your app in the Pubcenter site: http://pubcenter.microsoft.com 
with your LiveId

For getting more information about advanced options in the Pubcenter AdControl, please follow the next MSDN link: 
http://msdn.microsoft.com/en-us/library/advertising-mobile-windows-phone-8-adcontrol-visual-designer(v=msads.20).aspx      

