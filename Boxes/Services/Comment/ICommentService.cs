using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Services.Comment
{
    /// <summary>
    ///     Définit les méthodes pour accéder aux données de l'entité <see cref="Models.Comment"/>.
    /// </summary>
    public interface ICommentService
    {
        #region Create

        /// <summary>
        ///     Enregistre un nouveau commentaire.
        /// </summary>
        /// <param name="comment">
        ///     Commentaire à créer.
        /// </param>
        /// <returns>
        ///     Le commentaire fraichement créé.
        /// </returns>
        Task<Models.Comment> CreateAsync(Models.Comment comment);

        #endregion

        #region Read

        /// <summary>
        ///     Récupère les commentaire d'un post.
        /// </summary>
        /// <param name="post">
        ///     Post duquel récupérer les commentaire.
        /// </param>
        /// <returns>
        ///     Les commentaire du post.
        /// </returns>
        Task<List<Models.Comment>> GetByPostAsync(Models.Post post);

        #endregion
    }
}
