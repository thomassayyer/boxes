using Boxes.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace Boxes.Views
{
    /// <summary>
    ///     Page "coquille" contenant les autres pages.
    /// </summary>
    /// <remarks>
    ///     Cette page contient un menu hamburger permettant de naviguer vers les autres pages.
    /// </remarks>
    public sealed partial class Shell : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants de la page.
        /// </summary>
        public Shell()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private ShellViewModel ViewModel
        {
            get { return this.DataContext as ShellViewModel; }
        }

        /// <summary>
        ///     Représente la Frame sur laquelle naviguer.
        /// </summary>
        public Frame RootFrame => this.ContentFrame;

        #endregion

        #region Navigation

        /// <summary>
        ///     S'exécute lorsqu'une navigation s'effectue sur cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres relatifs à la navigation.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Navigue vers la page d'accueil et configure le view model pour qu'il reçoive les messages
            // qu'il est censé recevoir.
            this.ViewModel.Initialize();

            SystemNavigationManager.GetForCurrentView().BackRequested += this.Shell_BackRequested;

            base.OnNavigatedTo(e);
        }

        /// <summary>
        ///     S'exécute lorsqu'une navigation s'effectue à partir de cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres relatifs à la navigation.
        /// </param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Désabonne le view model de tous les messages auxquels il s'est abonné.
            this.ViewModel.Cleanup();

            SystemNavigationManager.GetForCurrentView().BackRequested -= this.Shell_BackRequested;

            base.OnNavigatedFrom(e);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Appelée lors du click sur le bouton hamburger.
        /// </summary>
        /// <param name="sender">
        ///     Bouton à l'origine de l'événement.
        /// </param>
        /// <param name="e">
        ///     Paramètres relatifs au click sur le bouton.
        /// </param>
        private void HamburgerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(RadioButton))
                ((RadioButton)sender).IsChecked = false;

            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        /// <summary>
        ///     Appelée lors de l'appui sur un bouton physique de retour arrière.
        /// </summary>
        /// <param name="sender">
        ///     Objet à l'origine de l'événement.
        /// </param>
        /// <param name="e">
        ///     Paramètres relatifs au click sur le bouton.
        /// </param>
        private void Shell_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.ViewModel.IsBackButtonVisible)
            {
                e.Handled = true;
                this.ViewModel.GoBackCommand.Execute(null);
            }
        }

        #endregion
    }
}
