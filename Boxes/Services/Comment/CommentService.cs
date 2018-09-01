using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Boxes.Auxiliary.Exceptions;

namespace Boxes.Services.Comment
{
    /// <summary>
    ///     Service d'accès aux données de l'entité <see cref="Models.Comment"/>.
    /// </summary>
    class CommentService : HttpService, ICommentService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer le commentaire créé ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.Comment> CreateAsync(Models.Comment comment)
        {
            var pairs = new Dictionary<string, string>
            {
                { "content", comment.Content },
                { "user_id", comment.Author.Id.ToString() },
                { "post_id", comment.Post.Id.ToString() }
            };

            HttpResponseMessage response = await this.PostAsync("comment", pairs);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<Models.Comment>(response.Content.ToString());
        }

        #endregion

        #region Read

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer les commentaires du post ou
        ///     que ce dernier renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<List<Models.Comment>> GetByPostAsync(Models.Post post)
        {
            HttpResponseMessage response = await this.GetAsync("post/" + post.Id + "/comments");

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<List<Models.Comment>>(response.Content.ToString());
        }

        #endregion
    }
}
