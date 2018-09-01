using Windows.Storage;

namespace Boxes.Services.Storage
{
    /// <summary>
    ///     Service d'accès aux données de stockage local.
    /// </summary>
    class StorageService : IStorageService
    {
        #region Fields

        /// <summary>
        ///     Conteneur de paramètres d'application.
        /// </summary>
        private ApplicationDataContainer localSettings;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la propriété <c>localSettings</c>.
        /// </summary>
        public StorageService()
        {
            this.localSettings = ApplicationData.Current.LocalSettings;
        }

        #endregion

        #region Save

        /// <inheritdoc />
        public void SaveSetting(string key, object value)
        {
            this.localSettings.Values[key] = value;
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public T ReadSetting<T>(string key) where T : class
        {
            return this.localSettings.Values[key] as T;
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public void RemoveSetting(string key)
        {
            this.localSettings.Values.Remove(key);
        }

        #endregion
    }
}
