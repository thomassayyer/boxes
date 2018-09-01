using Boxes.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page de création d'une boite.
    /// </summary>
    public sealed partial class CreateBoxPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants
        ///     de la page.
        /// </summary>
        public CreateBoxPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private CreateBoxViewModel ViewModel
        {
            get { return this.DataContext as CreateBoxViewModel; }
        }

        #endregion

        #region Navigation

        /// <summary>
        ///     S'exécute pour chaque navigation qui se fait vers cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres relatifs à la navigation effectuée.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel.Initialize();

            base.OnNavigatedTo(e);
        }

        /// <summary>
        ///     S'exécute pour chaque navigation effectuée à partir de cette page.
        /// </summary>
        /// <param name="e">
        ///     Paramètres relatifs à la navigation effectuée.
        /// </param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Cleanup();

            base.OnNavigatedFrom(e);
        }

        #endregion
    }
}
