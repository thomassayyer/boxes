using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;
using Boxes.Auxiliary.Exceptions;

namespace Boxes.Services.Post
{
    /// <summary>
    ///     Service d'accès aux données du model <see cref="Models.Post" />.
    /// </summary>
    class PostService : HttpService, IPostService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer le post créé ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.Post> CreateAsync(Models.Post post)
        {
            var pairs = new Dictionary<string, string>
            {
                { "content", post.Content },
                { "user_id", post.Author.Id.ToString() },
                { "box_id", post.Box.Id.ToString() }
            };

            HttpResponseMessage response = await this.PostAsync("post", pairs);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<Models.Post>(response.Content.ToString());
        }

        #endregion

        #region Read

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer les posts de la boite ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Post>> GetByBoxAsync(Models.Box box)
        {
            HttpResponseMessage response = await this.GetAsync("box/" + box.Id + "/posts");

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Post>>(response.Content.ToString());
        }

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer les posts des boites
        ///     auxquelles l'utilisateur s'est abonné et/ou qu'il a créées ou qu'il renvoi un code
        ///     d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Post>> GetByUserAsync(Models.User user)
        {
            HttpResponseMessage response = await this.GetAsync("user/" + user.Id + "/posts");

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Post>>(response.Content.ToString());
        }

        #endregion
    }
}
