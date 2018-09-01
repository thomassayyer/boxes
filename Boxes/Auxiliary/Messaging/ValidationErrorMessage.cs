using System.Collections.Generic;

namespace Boxes.Auxiliary.Messaging
{
    /// <summary>
    ///     Message d'erreur à envoyer lors d'une saisie non conforme.
    /// </summary>
    public class ValidationErrorMessage : GenericErrorMessage
    {
        /// <summary>
        ///     Erreurs relevées.
        /// </summary>
        public List<string> Errors { get; private set; }

        /// <summary>
        ///     Constructeur dont on spécifie le contenu du message ainsi que les erreurs
        ///     relevées lors du contrôle de saisie.
        /// </summary>
        /// <param name="content">
        ///     Contenu du message à transmettre.
        /// </param>
        /// <param name="errors">
        ///     Liste des erreurs à transmettre.
        /// </param>
        public ValidationErrorMessage(string content, List<string> errors) : base(content)
        {
            this.Errors = errors;
        }
    }
}
