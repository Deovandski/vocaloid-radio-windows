namespace WPAppStudio.Services.Interfaces
{
	/// <summary>
    /// Interface for a Storage service.
    /// </summary>
    public interface IStorageService
    {
        T Load<T>(string fileName);
        bool Save<T>(string fileName, T data);
    }
}
