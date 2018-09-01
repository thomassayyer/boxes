using Boxes.Services.Localization;
using System.Collections.Generic;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données fictives de localization.
    /// </summary>
    class FakeLocalizationService : ILocalizationService
    {
        #region Fields

        /// <summary>
        ///     Stock les resources fictives de localization.
        /// </summary>
        private readonly Dictionary<string, string> resources;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les resources fictives
        ///     de localization.
        /// </summary>
        public FakeLocalizationService()
        {
            this.resources = new Dictionary<string, string>();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public string GetString(string resourceKey)
        {
            if (this.resources.ContainsKey(resourceKey))
                return this.resources[resourceKey];

            return "Test";
        }

        #endregion
    }
}
