using GalaSoft.MvvmLight.Messaging;

namespace Boxes.Auxiliary.Messaging
{
    /// <summary>
    ///     Message d'erreur générique à envoyer pour toute erreur qui se produit pendant l'exécution.
    /// </summary>
    public class GenericErrorMessage : GenericMessage<string>
    {
        /// <summary>
        ///     Constructeur paramétré dont on spécifie le contenu du message.
        /// </summary>
        /// <param name="content">
        ///     Contenu du message d'erreur.
        /// </param>
        public GenericErrorMessage(string content) : base(content)
        { }
    }
}
