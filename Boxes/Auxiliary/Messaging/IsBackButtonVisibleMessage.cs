using GalaSoft.MvvmLight.Messaging;

namespace Boxes.Auxiliary.Messaging
{
    /// <summary>
    ///     Message qui détermine si le bouton de retour doit être visible ou non.
    /// </summary>
    public class IsBackButtonVisibleMessage : GenericMessage<bool>
    {
        /// <summary>
        ///     Constructeur non paramétré avec valeur par défaut.
        /// </summary>
        public IsBackButtonVisibleMessage() : base(true)
        { }

        /// <summary>
        ///     Constructeur dont on spécifie si le bouton est visible ou non.
        /// </summary>
        /// <param name="content">
        ///     Est-ce que le bouton est visible ?
        /// </param>
        public IsBackButtonVisibleMessage(bool content) : base(content)
        { }
    }
}
