using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Services.Post
{
    /// <summary>
    ///     Définit les méthodes pour accéder aux données de l'entité <see cref="Models.Post"/>.
    /// </summary>
    public interface IPostService
    {
        #region Create

        /// <summary>
        ///     Enregistre un nouveau post.
        /// </summary>
        /// <param name="post">
        ///     Post à créer.
        /// </param>
        /// <returns>
        ///     Le post fraichement créé.
        /// </returns>
        Task<Models.Post> CreateAsync(Models.Post post);

        #endregion

        #region Read

        /// <summary>
        ///     Récupère les post d'une boite en paramètre.
        /// </summary>
        /// <param name="box">
        ///     Boite de laquelle récupérer les posts.
        /// </param>
        /// <returns>
        ///     Liste des posts contenus dans la boite en paramètre.
        /// </returns>
        Task<List<Models.Post>> GetByBoxAsync(Models.Box box);

        /// <summary>
        ///     Récupère les posts des boites auxquelles l'utilisateur en paramètre s'est abonné et/ou qu'il
        ///     a créées.
        /// </summary>
        /// <param name="user">
        ///     Utilisateur duquel récupérer les posts.
        /// </param>
        /// <returns>
        ///     Liste des posts des boites auxquelles l'utilisateur en paramètre s'est abonné et/ou qu'il
        ///     a créées.
        /// </returns>
        Task<List<Models.Post>> GetByUserAsync(Models.User user);

        #endregion
    }
}
