using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Box;
using Boxes.Services.Localization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page d'édition d'une boite.
    /// </summary>
    public class EditBoxViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Box"/>.
        /// </summary>
        private readonly IBoxService boxService;

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
        ///     Stock la valeur de la propriété <c>IsUpdating</c>.
        /// </summary>
        private bool isUpdating;

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
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service d'accès aux données de localization.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service d'affichage de popups.
        /// </param>
        public EditBoxViewModel(IBoxService boxService, INavigationService navigationService,
            ILocalizationService localizationService, IDialogService dialogService)
        {
            this.boxService = boxService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.UpdateBoxCommand = new RelayCommand(this.UpdateBox, this.CanUpdateBoxExecute);

            this.IsUpdating = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Est-ce que la boite est en train d'être modifiée ?
        /// </summary>
        public bool IsUpdating
        {
            get { return this.isUpdating; }
            set
            {
                this.Set(() => this.IsUpdating, ref this.isUpdating, value);
                this.UpdateBoxCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Id de la boite.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Titre de la boite.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.Set(() => this.Title, ref this.title, value); }
        }

        /// <summary>
        ///     Description de la boite.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.Set(() => this.Description, ref this.description, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Commande d'édition de la boite.
        /// </summary>
        public RelayCommand UpdateBoxCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les opérations qui doivent s'exécuter pour chaque navigation
        ///     qui s'effectue sur la page d'édition d'une boite.
        /// </summary>
        /// <param name="box">
        ///     Boite à modifier.
        /// </param>
        public void Initialize(Box box)
        {
            this.Id = box.Id;
            this.Title = box.Title;
            this.Description = box.Description;

            // Demande l'affichage du bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage());

            // Demande le changement de titre sur le Shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.Title));
        }

        /// <summary>
        ///     Effectue toute les opérations qui doivent s'exécuter dès que
        ///     l'utilisateur quitte la page d'édition d'une boite.
        /// </summary>
        public override void Cleanup()
        {
            this.Title = null;
            this.Description = null;

            // Demande à cacher le bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage(false));

            base.Cleanup();
        }

        #endregion

        #region Update

        /// <summary>
        ///     Détermine si la commande d'édition d'une boite peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande d'édition d'une boite peut s'exécuter ?
        /// </returns>
        private bool CanUpdateBoxExecute()
        {
            return !this.IsUpdating &&
                   !string.IsNullOrWhiteSpace(this.Title) &&
                   !string.IsNullOrWhiteSpace(this.Description);
        }

        /// <summary>
        ///     Modifie la boite.
        /// </summary>
        private async void UpdateBox()
        {
            var box = new Box
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description
            };

            try
            {
                this.IsUpdating = true;

                await this.boxService.UpdateAsync(box);

                this.navigationService.GoBack();
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsUpdating = false;
            }
        }

        #endregion
    }
}
