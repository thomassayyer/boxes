using Boxes.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page d'inscription à Boxes.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants du view model.
        /// </summary>
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private RegisterViewModel ViewModel
        {
            get { return this.DataContext as RegisterViewModel; }
        }

        #endregion

        #region Navigation

        /// <summary>
        ///     S'exécute pour chaque navigation qui se fait vers cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres de la navigation effectuée.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += this.RegisterPage_BackRequested;

            base.OnNavigatedTo(e);
        }

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

        #region Events

        /// <summary>
        ///     Appelée lors de l'appui sur un bouton physique de retour arrière.
        /// </summary>
        /// <param name="sender">
        ///     Objet à l'origine de l'événement.
        /// </param>
        /// <param name="e">
        ///     Paramètres relatifs au click sur le bouton.
        /// </param>
        private void RegisterPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.GoBackCommand.Execute(null);
        }

        #endregion
    }
}
