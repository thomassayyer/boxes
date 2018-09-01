using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Boxes.Services
{
    /// <summary>
    ///     Classe de base pour tout les services ayant besoins de communiquer avec le WebService.
    /// </summary>
    abstract class HttpService
    {
        #region Constants

        /// <summary>
        ///     Clé d'API qui permet de se connecter au web service.
        /// </summary>
        private const string apiKey = "41q99xSwS8neFbuVoQULTY4GgjRHPVl6";

        /// <summary>
        ///     URL de base du web service.
        /// </summary>
        private const string baseUrl = "http://boxes.hol.es/public/api/" + apiKey;

        #endregion

        #region Post

        /// <summary>
        ///     Exécute une requête HTTP de type POST.
        /// </summary>
        /// <param name="url">
        ///     URL de l'action du service web à exécuter.
        /// </param>
        /// <param name="pairs">
        ///     Paramètres de la requête HTTP.
        /// </param>
        /// <returns>
        ///     Réponse du web service.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> pairs)
        {
            var client = new HttpClient();

            var formContent = new HttpFormUrlEncodedContent(pairs);

            return await client.PostAsync(new Uri(baseUrl + "/" + url), formContent);
        }

        #endregion

        #region Get

        /// <summary>
        ///     Exécute une requête HTTP de type GET.
        /// </summary>
        /// <param name="url">
        ///     URL de l'action du service à exécuter.
        /// </param>
        /// <returns>
        ///     Réponse du web service.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var client = new HttpClient();

            return await client.GetAsync(new Uri(baseUrl + "/" + url));
        }

        #endregion

        #region Put

        /// <summary>
        ///     Exécute une requête HTTP de type PUT.
        /// </summary>
        /// <param name="url">
        ///     URL de l'action du web service à exécuter.
        /// </param>
        /// <param name="pairs">
        ///     Paramètres de la requête HTTP.
        /// </param>
        /// <returns>
        ///     Réponse du web service.
        /// </returns>
        public async Task<HttpResponseMessage> PutAsync(string url, Dictionary<string, string> pairs)
        {
            var client = new HttpClient();

            var formContent = new HttpFormUrlEncodedContent(pairs);

            return await client.PutAsync(new Uri(baseUrl + "/" + url), formContent);
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Exécute une requête HTTP de type DELETE.
        /// </summary>
        /// <param name="url">
        ///     URL de l'action du web service à exécuter.
        /// </param>
        /// <returns>
        ///     Réponse du web service.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var client = new HttpClient();

            return await client.DeleteAsync(new Uri(baseUrl + "/" + url));
        }

        #endregion
    }
}
