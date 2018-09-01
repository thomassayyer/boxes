namespace Boxes.Services.Storage
{
    /// <summary>
    ///     Définit les méthodes pour accéder aux données de stockage local.
    /// </summary>
    public interface IStorageService
    {
        #region Save

        /// <summary>
        ///     Enregistre dans le conteneur de paramètres d'application une donnée.
        /// </summary>
        /// <param name="key">
        ///     Clé du paramètre.
        /// </param>
        /// <param name="value">
        ///     Valeur du paramètre.
        /// </param>
        void SaveSetting(string key, object value);

        #endregion

        #region Read

        /// <summary>
        ///     Retourne un paramètre du conteneur de paramètres d'application.
        /// </summary>
        /// <typeparam name="T">
        ///     Type de la donnée à retourner.
        /// </typeparam>
        /// <param name="key">
        ///     Clé du paramètre.
        /// </param>
        /// <returns>
        ///     Un paramètre du conteneur de paramètres d'application.
        /// </returns>
        T ReadSetting<T>(string key) where T : class;

        #endregion

        #region Delete

        /// <summary>
        ///     Supprime un paramètre du conteneur de paramètres d'application.
        /// </summary>
        /// <param name="key">
        ///     Clé du paramètre.
        /// </param>
        void RemoveSetting(string key);

        #endregion
    }
}
