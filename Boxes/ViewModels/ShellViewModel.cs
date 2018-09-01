using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Localization;
using Boxes.Services.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page de menu.
    /// </summary>
    public class ShellViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données de localization.
        /// </summary>
        private readonly ILocalizationService localizationService;

        /// <summary>
        ///     Stock le service d'accès aux données de stockage local.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Title</c>.
        /// </summary>
        private string title;

        /// <summary>
        ///     Stock la valeur de la propriété <c>CurrentUserName</c>.
        /// </summary>
        private string currentUserName;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsBackButtonVisible</c>.
        /// </summary>
        private bool isBackButtonVisible;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialise les propriétés du view model.
        /// </summary>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service d'accès aux données de localization.
        /// </param>
        /// <param name="storageService">
        ///     Instance du service d'accès aux données de stockage local.
        /// </param>
        public ShellViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IStorageService storageService)
        {
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.storageService = storageService;

            this.GoBackCommand = new RelayCommand(this.navigationService.GoBack);
            this.NavigateToHomeCommand = new RelayCommand(this.NavigateToHome);
            this.NavigateToDiscoverCommand = new RelayCommand(this.NavigateToDiscover);
            this.NavigateToMyBoxesCommand = new RelayCommand(this.NavigateToMyBoxes);
            this.SignoutCommand = new RelayCommand(this.Signout);

            this.IsBackButtonVisible = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Titre de la page en cours.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.Set(() => this.Title, ref this.title, value); }
        }

        /// <summary>
        ///     Prénom et nom de l'utilisateur courant.
        /// </summary>
        public string CurrentUserName
        {
            get { return this.currentUserName; }
            set { this.Set(() => this.CurrentUserName, ref this.currentUserName, value); }
        }

        /// <summary>
        ///     Est-ce que le bouton de retour arrière est visible ?
        /// </summary>
        public bool IsBackButtonVisible
        {
            get { return this.isBackButtonVisible; }
            set { this.Set(() => this.IsBackButtonVisible, ref this.isBackButtonVisible, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande de retour arrière.
        /// </summary>
        public RelayCommand GoBackCommand { get; private set; }

        /// <summary>
        ///     Commande de navigation vers la page d'accueil.
        /// </summary>
        public RelayCommand NavigateToHomeCommand { get; private set; }

        /// <summary>
        ///     Commande de navigation vers la page "Découvrir".
        /// </summary>
        public RelayCommand NavigateToDiscoverCommand { get; private set; }

        /// <summary>
        ///     Commande de navigation vers la page "Mes boites".
        /// </summary>
        public RelayCommand NavigateToMyBoxesCommand { get; private set; }

        /// <summary>
        ///     Commande de déconnexion.
        /// </summary>
        public RelayCommand SignoutCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Enregistre la réception des message de type <see cref="IsBackButtonVisibleMessage"/>
        ///     et <see cref="ShellTitleMessage"/>. Navigue également vers la page par défaut et charge
        ///     l'utilisateur par défaut dans <c>CurrentUserName</c>.
        /// </summary>
        public void Initialize()
        {
            this.MessengerInstance.Register<IsBackButtonVisibleMessage>(this, this.HandleIsBackButtonVisibleMessage);
            this.MessengerInstance.Register<ShellTitleMessage>(this, this.HandleShellTitleMessage);

            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));
            this.CurrentUserName = user.ToString();

            this.NavigateToHome();
        }

        #endregion

        #region Navigation

        /// <summary>
        ///     Navigue vers la page d'accueil.
        /// </summary>
        private void NavigateToHome()
        {
            this.navigationService.NavigateTo("Home");
        }

        /// <summary>
        ///     Navigue vers la page "Découvrir".
        /// </summary>
        private void NavigateToDiscover()
        {
            this.navigationService.NavigateTo("Discover");
        }

        /// <summary>
        ///     Navigue vers la page d'administration "Mes boites".
        /// </summary>
        private void NavigateToMyBoxes()
        {
            this.navigationService.NavigateTo("MyBoxes");
        }

        #endregion

        #region Messages handling

        /// <summary>
        ///     Traite un message concernant la visibilité du bouton de retour.
        /// </summary>
        /// <param name="m">
        ///     Message à traiter.
        /// </param>
        private void HandleIsBackButtonVisibleMessage(IsBackButtonVisibleMessage m)
        {
            this.IsBackButtonVisible = m.Content;
        }

        /// <summary>
        ///     Traite un message contenant un titre pour le Shell.
        /// </summary>
        /// <param name="m">
        ///     Message à traiter.
        /// </param>
        private void HandleShellTitleMessage(ShellTitleMessage m)
        {
            this.Title = m.Content;
        }

        #endregion

        #region Signout

        /// <summary>
        ///     Déconnecte l'utilisateur courant.
        /// </summary>
        private void Signout()
        {
            this.storageService.RemoveSetting("CurrentUser");
            this.navigationService.NavigateTo("Login");
        }

        #endregion
    }
}
