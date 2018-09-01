using GalaSoft.MvvmLight.Views;
using System;
using Windows.UI.Popups;
using System.Threading.Tasks;

namespace Boxes.Services.Dialog
{
    /// <summary>
    ///     Gère l'affichage de popups.
    /// </summary>
    class DialogService : IDialogService
    {
        #region ShowError

        /// <inheritdoc />
        public async Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            await this.ShowError(error.Message, title, buttonText, afterHideCallback);
        }

        /// <inheritdoc />
        public async Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(buttonText));

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            await dialog.ShowAsync();

            if (afterHideCallback != null)
                afterHideCallback();
        }

        #endregion

        #region ShowMessage

        /// <inheritdoc />
        public async Task ShowMessage(string message, string title)
        {
            await new MessageDialog(message, title).ShowAsync();
        }

        /// <inheritdoc />
        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(buttonText));

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            await dialog.ShowAsync();

            if (afterHideCallback != null)
                afterHideCallback();
        }

        /// <inheritdoc />
        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(buttonConfirmText) { Id = 1 });
            dialog.Commands.Add(new UICommand(buttonCancelText) { Id = 2 });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            IUICommand command = await dialog.ShowAsync();

            if (afterHideCallback != null)
                afterHideCallback((int)command.Id == 1);

            return (int)command.Id == 1;
        }

        #endregion

        #region ShowMessageBox

        /// <inheritdoc />
        public async Task ShowMessageBox(string message, string title)
        {
            await new MessageDialog(message, title).ShowAsync();
        }

        #endregion
    }
}
