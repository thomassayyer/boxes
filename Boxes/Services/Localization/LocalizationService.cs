using Windows.ApplicationModel.Resources;

namespace Boxes.Services.Localization
{
    /// <summary>
    ///     Service d'accès aux données de localization.
    /// </summary>
    class LocalizationService : ILocalizationService
    {
        #region Fields

        /// <summary>
        ///     Stock l'objet d'accès aux resources de localization.
        /// </summary>
        private ResourceLoader resourceLoader;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la propriété <c>resourceLoader</c>.
        /// </summary>
        public LocalizationService()
        {
            this.resourceLoader = ResourceLoader.GetForCurrentView();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public string GetString(string resourceKey)
        {
            return this.resourceLoader.GetString(resourceKey);
        }

        #endregion
    }
}
