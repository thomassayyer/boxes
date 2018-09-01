using Boxes.Auxiliary.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Boxes.Services.User
{
    /// <summary>
    ///     Service d'accès aux données de l'entité <see cref="Models.User"/>.
    /// </summary>
    class UserService : HttpService, IUserService
    {
        #region Create
        
        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer l'utilisateur créé
        ///     ou qu'il renvoi un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.User> CreateAsync(Models.User user)
        {
            var pairs = new Dictionary<string, string>
            {
                { "first_name", user.FirstName },
                { "last_name", user.LastName },
                { "email", user.Email },
                { "password", user.Password },
                { "birth_date", user.BirthDate.ToString("yyyy-MM-dd") }
            };

            if (!string.IsNullOrEmpty(user.Phone))
                pairs["phone"] = user.Phone;

            HttpResponseMessage response = await this.PostAsync("user/register", pairs);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ToString()))
                throw new WebServiceException();

            return JsonConvert.DeserializeObject<Models.User>(response.Content.ToString());
        }

        #endregion

        #region Read

        /// <inheritdoc />
        /// <exception cref="WebServiceException">
        ///     Levée si le web service n'est pas en mesure de nous délivrer l'utilisateur
        ///     correspondant à l'email et le mot de passe en paramètre ou qu'il renvoi
        ///     un code d'erreur (500, 503, 404, ...).
        /// </exception>
        public async Task<Models.User> GetByEmailPasswordAsync(string email, string password)
        {
            HttpResponseMessage response = await this.GetAsync("user/login/" + email + "/" + password);

            if (!response.IsSuccessStatusCode)
                throw new WebServiceException();

            if (string.IsNullOrEmpty(response.Content.ToString()))
                return null;

            return JsonConvert.DeserializeObject<Models.User>(response.Content.ToString());
        }

        #endregion
    }
}
