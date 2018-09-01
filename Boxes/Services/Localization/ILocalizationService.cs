namespace Boxes.Services.Localization
{
    /// <summary>
    ///     Définit des méthodes pour manipuler les resources de localization.
    /// </summary>
    public interface ILocalizationService
    {
        #region Read

        /// <summary>
        ///     Récupère une donnée de localization en fonction d'une clé précisée.
        /// </summary>
        /// <param name="resourceKey">
        ///     Clé de la resource à récupérer.
        /// </param>
        /// <returns>
        ///     Valeur de la resource correspondante.
        /// </returns>
        string GetString(string resourceKey);

        #endregion
    }
}
