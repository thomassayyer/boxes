using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Boxes.Auxiliary.Exceptions;

namespace Boxes.Services.Box
{
    /// <summary>
    ///     Service d'accès aux données de l'entité <see cref="Models.Box"/>.
    /// </summary>
    class BoxService : HttpService, IBoxService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer la boite créée ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.Box> CreateAsync(Models.Box box)
        {
            var pairs = new Dictionary<string, string>
            {
                { "title", box.Title },
                { "description", box.Description },
                { "user_id", box.Creator.Id.ToString() }
            };

            HttpResponseMessage response = await this.PostAsync("box", pairs);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<Models.Box>(response.Content.ToString());
        }

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task AttachUserAsync(Models.Box box, Models.User user)
        {
            var pairs = new Dictionary<string, string>
            {
                { "userId", user.Id.ToString() },
                { "boxId", box.Id.ToString() }
            };

            HttpResponseMessage response = await this.PostAsync("user/subscribe", pairs);

            if (!response.IsSuccessStatusCode)
                throw new WebServiceException();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer les boites correspondantes à l'utilisateur ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Box>> GetByUserAsync(Models.User user)
        {
            HttpResponseMessage response = await this.GetAsync("user/" + user.Id + "/boxes");

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Box>>(response.Content.ToString());
        }

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer le top des boites ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Box>> GetTopAsync()
        {
            HttpResponseMessage response = await this.GetAsync("box/top");

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Box>>(response.Content.ToString());
        }

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer le résultat de la recherche ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Box>> GetSearchResultsAsync(string terms)
        {
            HttpResponseMessage response = await this.GetAsync("box/search/" + terms);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Box>>(response.Content.ToString());
        }

        #endregion

        #region Update

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.Box> UpdateAsync(Models.Box box)
        {
            var pairs = new Dictionary<string, string>
            {
                { "title", box.Title },
                { "description", box.Description }
            };

            HttpResponseMessage response = await this.PutAsync("box/" + box.Id, pairs);

            if (!response.IsSuccessStatusCode)
                throw new WebServiceException();

            if (string.IsNullOrEmpty(response.Content.ToString()))
                return null;

            return JsonConvert.DeserializeObject<Models.Box>(response.Content.ToString());
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task DetachUserAsync(Models.Box box, Models.User user)
        {
            HttpResponseMessage response = await this.DeleteAsync("user/" + user.Id + "/unsubscribe/" + box.Id);

            if (!response.IsSuccessStatusCode)
                throw new WebServiceException();
        }

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task DeleteAsync(Models.Box box)
        {
            HttpResponseMessage response = await this.DeleteAsync("box/" + box.Id);

            if (!response.IsSuccessStatusCode)
                throw new WebServiceException();
        }

        #endregion
    }
}
