using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using WPAppStudio.Services.Interfaces;

namespace WPAppStudio.Services
{
    /// <summary>
    /// Implementation of a storage service.
    /// </summary>
    public class StorageService : IStorageService
    {
	    /// <summary>
        /// Load data from isolated storage.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        public T Load<T>(string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.FileExists(fileName))
                {
                    return default(T);
                }

                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(fileName, FileMode.Open))
                {
                    var xml = new XmlSerializer(typeof (T));
                    var data = (T) xml.Deserialize(stream);
                    return data;
                }
            }
        }

		/// <summary>
        /// Save data to isolated storage.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
	    /// <param name="data">The data to save.</param>
        public bool Save<T>(string fileName, T data)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName))
                    {
                        var xml = new XmlSerializer(typeof (T));
                        xml.Serialize(fileStream, data);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}