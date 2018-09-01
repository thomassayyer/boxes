using Boxes.Services.Storage;
using System.Collections.Generic;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données d'un stockage local fictif.
    /// </summary>
    class FakeStorageService : IStorageService
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise le conteneur fictif de
        ///     paramètres d'application.
        /// </summary>
        public FakeStorageService()
        {
            this.LocalSettings = new Dictionary<string, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Conteneur fictif de paramètres d'application.
        /// </summary>
        public Dictionary<string, object> LocalSettings { get; private set; }

        #endregion

        #region Save

        /// <inheritdoc />
        public void SaveSetting(string key, object value)
        {
            this.LocalSettings[key] = value;
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public T ReadSetting<T>(string key) where T : class
        {
            if (!this.LocalSettings.ContainsKey(key))
                return null;

            return this.LocalSettings[key] as T;
        }

        #endregion

        #region Remove

        /// <inheritdoc />
        public void RemoveSetting(string key)
        {
            if (this.LocalSettings.ContainsKey(key))
                this.LocalSettings.Remove(key);
        }

        #endregion
    }
}
