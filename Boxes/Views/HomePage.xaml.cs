using Boxes.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page d'accueil qui affiche les posts des boites auxquelles l'utilisateur s'est
    ///     abonnées et/ou qu'il a créées.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants de la
        ///     page.
        /// </summary>
        public HomePage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private HomeViewModel ViewModel
        {
            get { return this.DataContext as HomeViewModel; }
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
            this.ViewModel.Initialize();

            base.OnNavigatedTo(e);
        }

        #endregion
    }
}
