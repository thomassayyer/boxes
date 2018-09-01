using GalaSoft.MvvmLight.Messaging;

namespace Boxes.Auxiliary.Messaging
{
    /// <summary>
    ///     Message contenant le titre du Shell.
    /// </summary>
    public class ShellTitleMessage : GenericMessage<string>
    {
        /// <summary>
        ///     Constructeur paramétré dont on spécifie titre du Shell.
        /// </summary>
        /// <param name="content">
        ///     Contenu du message.
        /// </param>
        public ShellTitleMessage(string content) : base(content)
        { }
    }
}
