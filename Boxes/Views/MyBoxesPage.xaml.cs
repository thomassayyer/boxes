using Boxes.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Views
{
    /// <summary>
    ///     Page contenant les boites que l'utilisateur a créées et/ou auxquelles il
    ///     s'est abonnées.
    /// </summary>
    public sealed partial class MyBoxesPage : Page
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les composants de
        ///     la page.
        /// </summary>
        public MyBoxesPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     View model de la page.
        /// </summary>
        private MyBoxesViewModel ViewModel
        {
            get { return this.DataContext as MyBoxesViewModel; }
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

        #endregion
    }
}
