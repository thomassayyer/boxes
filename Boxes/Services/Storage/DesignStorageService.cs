using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Boxes.Services.Storage
{
    /// <summary>
    ///     Service d'accès aux données de design de stockage local.
    /// </summary>
    /// <remarks>
    ///     Retourne des données fictives contenu dans une collection.
    /// </remarks>
    class DesignStorageService : IStorageService
    {
        #region Fields

        /// <summary>
        ///     Conteneur fictif de paramètres d'application.
        /// </summary>
        private Dictionary<string, object> localSettings;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui enregistre les données fictives
        ///     de la propriété <c>localSettings</c>.
        /// </summary>
        public DesignStorageService()
        {
            this.localSettings = new Dictionary<string, object>
            {
                {
                    "CurrentUser",
                    JsonConvert.SerializeObject(new Models.User
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        BirthDate = new DateTime(1970, 1, 1)
                    })
                }
            };
        }

        #endregion

        #region Save

        /// <inheritdoc />
        public void SaveSetting(string key, object value)
        {
            this.localSettings[key] = value;
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public T ReadSetting<T>(string key) where T : class
        {
            return this.localSettings[key] as T;
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public void RemoveSetting(string key)
        {
            this.localSettings.Remove(key);
        }

        #endregion
    }
}
