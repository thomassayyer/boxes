using System.Threading.Tasks;

namespace Boxes.Services.User
{
    /// <summary>
    ///     Définit les méthode d'accès aux données de l'entité <see cref="Models.User"/>.
    /// </summary>
    public interface IUserService
    {
        #region Create

        /// <summary>
        ///     Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="user">
        ///     Utilisateur à créer.
        /// </param>
        /// <returns>
        ///     L'utilisateur créé.
        /// </returns>
        Task<Models.User> CreateAsync(Models.User user);

        #endregion

        #region Read

        /// <summary>
        ///     Récupère un utilisateur en fonction de son email et de son mot de passe.
        /// </summary>
        /// <param name="email">
        ///     Email de l'utilisateur.
        /// </param>
        /// <param name="password">
        ///     Mot de passe de l'utilisateur.
        /// </param>
        /// <returns>
        ///     L'utilisateur qui a l'email et le mot de passe en paramètre.
        /// </returns>
        Task<Models.User> GetByEmailPasswordAsync(string email, string password);

        #endregion
    }
}
