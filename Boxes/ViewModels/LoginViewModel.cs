using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Boxes.Services.User;
using Boxes.Services.Storage;
using Boxes.Models;
using Boxes.Auxiliary.Exceptions;
using Boxes.Services.Localization;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page de connexion.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de localization.
        /// </summary>
        private readonly ILocalizationService localizationService;

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="User"/>.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        ///     Stock le service d'accès aux données de stockage local.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        ///     Stock le service gérant l'affichage de popups.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Email</c>.
        /// </summary>
        private string email;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Password</c>.
        /// </summary>
        private string password;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsLoggingIn</c>.
        /// </summary>
        private bool isLoggingIn;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialise les propriétés du view model.
        /// </summary>
        /// <param name="userService">
        ///     Instance du service des utilisateurs.
        /// </param>
        /// <param name="storageService">
        ///     Instance du service de stockage.
        /// </param>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service d'accès aux données de localization.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service gérant l'affichage de popups.
        /// </param>
        public LoginViewModel(IUserService userService, IStorageService storageService,
            INavigationService navigationService, ILocalizationService localizationService,
            IDialogService dialogService)
        {
            this.userService = userService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.LoginCommand = new RelayCommand(this.Login, this.CanLoginExecute);
            this.RegisterCommand = new RelayCommand(this.Register);

            this.IsLoggingIn = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Est-ce que la connexion est en cours ?
        /// </summary>
        public bool IsLoggingIn
        {
            get { return this.isLoggingIn; }
            set
            {
                this.Set(() => this.IsLoggingIn, ref this.isLoggingIn, value);
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Adresse email entrée par l'utilisateur.
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Mot de pass entré par l'utilisateur.
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande s'exécutant lors du click sur le bouton de connexion.
        /// </summary>
        public RelayCommand LoginCommand { get; private set; }

        /// <summary>
        ///     Commande s'exécutant lors du click sur le lien hypertext "S'enregistrer".
        /// </summary>
        public RelayCommand RegisterCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue toute les opération qui s'exécute dès que l'utilisateur
        ///     quite la page de connexion.
        /// </summary>
        public override void Cleanup()
        {
            this.Email = null;
            this.Password = null;

            base.Cleanup();
        }

        #endregion

        #region Login

        /// <summary>
        ///     Détermine si la commande de connexion peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande de connexion peut s'exécuter ?
        /// </returns>
        private bool CanLoginExecute()
        {
            return !string.IsNullOrWhiteSpace(this.Email) &&
                   !string.IsNullOrWhiteSpace(this.Password) &&
                   !this.IsLoggingIn;
        }

        /// <summary>
        ///     Connecte l'utilisateur à l'application.
        /// </summary>
        private async void Login()
        {
            try
            {
                this.IsLoggingIn = true;
                User user = await this.userService.GetByEmailPasswordAsync(this.Email, this.Password);

                if (user == null)
                {
                    await this.dialogService.ShowError(
                        this.localizationService.GetString("InvalidCredentials"),
                        "Hu hu !",
                        "Ok",
                        null
                    );
                    return;
                }

                // Ajout de l'utilisateur dans les "local settings".
                this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

                // Redirection vers la page de menu.
                this.navigationService.NavigateTo("Shell");
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsLoggingIn = false;
            }
        }

        #endregion

        #region Register

        /// <summary>
        ///     Redirige l'utilisateur vers la page d'inscription.
        /// </summary>
        private void Register()
        {
            this.navigationService.NavigateTo("Register");
        }

        #endregion
    }
}
