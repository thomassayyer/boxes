using Boxes.Models;
using Boxes.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page de détails concernant une boite.
    /// </summary>
    public sealed partial class BoxPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants de la
        ///     page.
        /// </summary>
        public BoxPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private BoxViewModel ViewModel
        {
            get { return this.DataContext as BoxViewModel; }
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
            this.ViewModel.Initialize((Box)e.Parameter);

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
    }
}
