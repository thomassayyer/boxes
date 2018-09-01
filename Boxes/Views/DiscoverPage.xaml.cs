using Boxes.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page de découverte des boites les plus populaires avec fonction de recherche.
    /// </summary>
    public sealed partial class DiscoverPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétrés qui initialise les composant
        ///     de la page.
        /// </summary>
        public DiscoverPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private DiscoverViewModel ViewModel
        {
            get { return this.DataContext as DiscoverViewModel; }
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
