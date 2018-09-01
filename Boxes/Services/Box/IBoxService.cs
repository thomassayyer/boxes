using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Services.Box
{
    /// <summary>
    ///     Définit les méthodes pour accéder aux données de l'entité <see cref="Models.Box"/>.
    /// </summary>
    public interface IBoxService
    {
        #region Create

        /// <summary>
        ///     Enregistre une nouvelle boite.
        /// </summary>
        /// <param name="box">
        ///     Boite à créer.
        /// </param>
        /// <returns>
        ///     Boite créée.
        /// </returns>
        Task<Models.Box> CreateAsync(Models.Box box);

        /// <summary>
        ///     Enregistre un utilisateur à la liste des abonnés d'une boite.
        /// </summary>
        /// <param name="box">
        ///     Boite à laquelle l'utilisateur s'abonne.
        /// </param>
        /// <param name="user">
        ///     Utilisateur voulant s'abonner.
        /// </param>
        /// <returns>
        ///     Opération asynchrone permettant d'attendre la fin des opérations.
        /// </returns>
        Task AttachUserAsync(Models.Box box, Models.User user);

        #endregion

        #region Read

        /// <summary>
        ///     Récupère toutes les boites auxquelles un utilisateur s'est abonné et/ou celles qu'il a créées.
        /// </summary>
        /// <param name="user">
        ///     Utilisateur duquel récupérer les boites.
        /// </param>
        /// <returns>
        ///     Liste des boites auxquelles l'utilisateur s'est abonné et/ou qu'il a créées.
        /// </returns>
        Task<List<Models.Box>> GetByUserAsync(Models.User user);

        /// <summary>
        ///     Récupère les boites les plus populaires c-à-d. les 10 boites ayant le plus d'abonnés.
        /// </summary>
        /// <returns>
        ///     Liste des boites les plus populaires.
        /// </returns>
        Task<List<Models.Box>> GetTopAsync();

        /// <summary>
        ///     Récupère le résultat d'une recherche sur des boites.
        /// </summary>
        /// <param name="terms">
        ///     Termes de la recherche.
        /// </param>
        /// <returns>
        ///     Liste des boites correspondantes aux termes de la recherche.
        /// </returns>
        Task<List<Models.Box>> GetSearchResultsAsync(string terms);

        #endregion

        #region Update

        /// <summary>
        ///     Modifie la boite en paramètre.
        /// </summary>
        /// <param name="box">
        ///     Boite à modifier.
        /// </param>
        /// <returns>
        ///     Boite modifiée.
        /// </returns>
        Task<Models.Box> UpdateAsync(Models.Box box);

        #endregion

        #region Delete

        /// <summary>
        ///     Supprime un utilisateur de la liste des abonnés d'une boite.
        /// </summary>
        /// <param name="box">
        ///     Boite de laquelle l'utilisateur se désabonne.
        /// </param>
        /// <param name="user">
        ///     Utilisateur voulant se désabonner.
        /// </param>
        /// <returns>
        ///     Opération asynchrone permettant d'attendre la fin des opérations.
        /// </returns>
        Task DetachUserAsync(Models.Box box, Models.User user);

        /// <summary>
        ///     Supprime la boite en paramètre.
        /// </summary>
        /// <param name="box">
        ///     Boite à supprimer.
        /// </param>
        /// <returns>
        ///     Opération asynchrone permettant d'attendre la fin des opérations.
        /// </returns>
        Task DeleteAsync(Models.Box box);

        #endregion
    }
}
