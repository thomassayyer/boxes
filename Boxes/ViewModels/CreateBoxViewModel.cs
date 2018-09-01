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

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page de création d'une boite.
    /// </summary>
    public class CreateBoxViewModel : ViewModelBase
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
        ///     Stock la valeur de la propriété <c>IsCreating</c>.
        /// </summary>
        private bool isCreating;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Title</c>.
        /// </summary>
        private string title;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Description</c>.
        /// </summary>
        private string description;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialise les propriétés du view model.
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
        public CreateBoxViewModel(IBoxService boxService, IStorageService storageService, INavigationService navigationService,
            ILocalizationService localizationService, IDialogService dialogService)
        {
            this.boxService = boxService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.CreateBoxCommand = new RelayCommand(this.CreateBox, this.CanCreateBoxExecute);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Est-ce que la boite est en train de se créer ?
        /// </summary>
        public bool IsCreating
        {
            get { return this.isCreating; }
            set
            {
                this.Set(() => this.IsCreating, ref this.isCreating, value);
                this.CreateBoxCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Titre de la boite.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.CreateBoxCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Description de la boite.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.CreateBoxCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Commande de création d'une boite.
        /// </summary>
        public RelayCommand CreateBoxCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialization pour chaque navigation qui se fait
        ///     sur la page de création d'une boite.
        /// </summary>
        public void Initialize()
        {
            // Demande l'affichage du bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage());

            this.MessengerInstance.Send(new ShellTitleMessage(this.localizationService.GetString("CreateBox")));
        }

        /// <summary>
        ///     Effectue les opération qui doivent s'exécuter à chaque fois que
        ///     l'utilisateur quite la page de création de la boite.
        /// </summary>
        public override void Cleanup()
        {
            // Demande à cacher le bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage(false));

            this.Title = null;
            this.Description = null;

            base.Cleanup();
        }

        #endregion

        #region Create

        /// <summary>
        ///     Détermine si la commande de création d'une boite peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande de création peut s'exécuter ?
        /// </returns>
        private bool CanCreateBoxExecute()
        {
            return !this.IsCreating &&
                   !string.IsNullOrWhiteSpace(this.Title) &&
                   !string.IsNullOrWhiteSpace(this.Description);
        }

        /// <summary>
        ///     Crée une nouvelle boite.
        /// </summary>
        private async void CreateBox()
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));
            var box = new Box
            {
                Title = this.Title,
                Description = this.Description,
                Creator = user
            };

            try
            {
                this.IsCreating = true;

                await this.boxService.CreateAsync(box);

                this.navigationService.GoBack();
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsCreating = false;
            }
        }

        #endregion
    }
}
