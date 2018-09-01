using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service fictif d'affichage de popups.
    /// </summary>
    class FakeDialogService : IDialogService
    {
        #region Properties

        /// <summary>
        ///     Dernière méthode appelée.
        /// </summary>
        public string MethodCalled { get; private set; }

        #endregion

        #region ShowError

        /// <inheritdoc />
        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            Task task = Task.Run(() => this.MethodCalled = "ShowError");
            task.Wait();

            if (afterHideCallback != null)
                afterHideCallback();

            return task;
        }

        /// <inheritdoc />
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            Task task = Task.Run(() => this.MethodCalled = "ShowError");
            task.Wait();

            if (afterHideCallback != null)
                afterHideCallback();

            return task;
        }

        #endregion

        #region ShowMessage

        /// <inheritdoc />
        public Task ShowMessage(string message, string title)
        {
            Task task = Task.Run(() => this.MethodCalled = "ShowMessage");
            task.Wait();

            return task;
        }

        /// <inheritdoc />
        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            Task task = Task.Run(() => this.MethodCalled = "ShowMessage");
            task.Wait();

            if (afterHideCallback != null)
                afterHideCallback();

            return task;
        }

        /// <inheritdoc />
        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            Task<bool> task = Task.FromResult(true);
            task.Wait();

            this.MethodCalled = "ShowMessage";

            if (afterHideCallback != null)
                afterHideCallback(true);

            return task;
        }

        #endregion

        #region ShowMessageBox

        /// <inheritdoc />
        public Task ShowMessageBox(string message, string title)
        {
            Task task = Task.Run(() => this.MethodCalled = "ShowMessageBox");
            task.Wait();

            return task;
        }

        #endregion
    }
}
