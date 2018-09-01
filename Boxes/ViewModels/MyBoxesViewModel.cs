using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Box;
using Boxes.Services.Localization;
using Boxes.Services.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page "Mes boites".
    /// </summary>
    public class MyBoxesViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Box"/>.
        /// </summary>
        private readonly IBoxService boxService;

        /// <summary>
        ///     Stock le service d'accès aux données de stockage local.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;
        
        /// <summary>
        ///     Stock le service d'accès aux données de localization.
        /// </summary>
        private readonly ILocalizationService localizationService;

        /// <summary>
        ///     Stock le service d'affichage de popups.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsLoading</c>.
        /// </summary>
        private bool isLoading;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Boxes</c>.
        /// </summary>
        private ObservableCollection<Box> boxes;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialize les propriétés du view model.
        /// </summary>
        /// <param name="boxService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Box"/>.
        /// </param>
        /// <param name="storageService">
        ///     Instance du service d'accès aux données de stockage local.
        /// </param>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service d'accès aux données de localization.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service d'affichage de popups.
        /// </param>
        public MyBoxesViewModel(IBoxService boxService, IStorageService storageService,
            INavigationService navigationService, ILocalizationService localizationService,
            IDialogService dialogService)
        {
            this.boxService = boxService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.ShowBoxCommand = new RelayCommand<Box>(this.ShowBox);
            this.ShowCreateBoxCommand = new RelayCommand(this.ShowCreateBox);

            this.IsLoading = false;
            this.Boxes = new ObservableCollection<Box>();

            if (this.IsInDesignMode)
                this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Est-ce que la page est en train de charger ?
        /// </summary>
        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        /// <summary>
        ///     Boites auxquelles l'utilisateur s'est abonnées et/ou qu'il a créées.
        /// </summary>
        public ObservableCollection<Box> Boxes
        {
            get { return this.boxes; }
            set { this.Set(() => this.Boxes, ref this.boxes, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande d'affichage d'une boite.
        /// </summary>
        public RelayCommand<Box> ShowBoxCommand { get; private set; }

        /// <summary>
        ///     Commande d'affichage du formulaire de création d'une boite.
        /// </summary>
        public RelayCommand ShowCreateBoxCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation pour chaque navigation qui se fait
        ///     sur la page "Mes boites".
        /// </summary>
        public void Initialize()
        {
            this.ReloadBoxes();

            // Demande le changement de titre du shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.localizationService.GetString("MyBoxes")));
        }

        #endregion

        #region Boxes

        /// <summary>
        /// Recharge les boites de l'utilisateur.
        /// </summary>
        private async void ReloadBoxes()
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            try
            {
                this.IsLoading = true;

                this.Boxes = new ObservableCollection<Box>(await this.boxService.GetByUserAsync(user));
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Navigue vers la page de détails d'une boite.
        /// </summary>
        /// <param name="box">Boite à afficher.</param>
        private void ShowBox(Box box)
        {
            this.navigationService.NavigateTo("Box", box);
        }

        /// <summary>
        /// Navigue vers le formulaire de création d'une boite.
        /// </summary>
        private void ShowCreateBox()
        {
            this.navigationService.NavigateTo("CreateBox");
        }

        #endregion
    }
}
