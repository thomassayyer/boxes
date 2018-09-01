using Boxes.Auxiliary.Messaging;
using Boxes.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page de connexion à Boxes.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants de
        ///     la page.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private LoginViewModel ViewModel
        {
            get { return this.DataContext as LoginViewModel; }
        }

        #endregion

        #region Navigation

        /// <summary>
        ///     S'exécute pour chaque navigation effectuée à partir de cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres de la navigation effectuée.
        /// </param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Cleanup();

            base.OnNavigatedFrom(e);
        }

        #endregion
    }
}
