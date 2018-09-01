using System;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Boxes.Models;
using Boxes.Services.User;
using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Services.Localization;
using Boxes.Auxiliary.Validation;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page d'inscription.
    /// </summary>
    public class RegisterViewModel : ValidatableViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="User"/>.
        /// </summary>
        private readonly IUserService userService;

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
        ///     Stock la valeur de la propriété <c>FirstName</c>.
        /// </summary>
        private string firstName;

        /// <summary>
        ///     Stock la valeur de la propriété <c>LastName</c>.
        /// </summary>
        private string lastName;

        /// <summary>
        ///     Stock la valeur de la propriété <c>BirthDate</c>.
        /// </summary>
        private DateTimeOffset? birthDate;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Email</c>.
        /// </summary>
        private string email;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Password</c>.
        /// </summary>
        private string password;

        /// <summary>
        ///     Stock la valeur de la propriété <c>PasswordConfirmation</c>.
        /// </summary>
        private string passwordConfirmation;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsRegistering</c>.
        /// </summary>
        private bool isRegistering;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialize les propriétés du view model.
        /// </summary>
        /// <param name="userService">
        ///     Instance du service d'accès aux données de l'entité <see cref="User"/>.
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
        public RegisterViewModel(IUserService userService, INavigationService navigationService,
            ILocalizationService localizationService, IDialogService dialogService)
        {
            this.userService = userService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.RegisterCommand = new RelayCommand(this.Register, this.CanRegisterExecute);
            this.GoBackCommand = new RelayCommand(this.navigationService.GoBack);

            this.IsRegistering = false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Prénom entré par l'utilisateur.
        /// </summary>
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                if (this.firstName != value)
                {
                    this.firstName = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Nom entré par l'utilisateur.
        /// </summary>
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                if (this.lastName != value)
                {
                    this.lastName = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Numéro de tél. entré par l'utilisateur.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Date de naissance entrée par l'utilisateur.
        /// </summary>
        public DateTimeOffset? BirthDate
        {
            get { return this.birthDate; }
            set
            {
                if (this.birthDate != value)
                {
                    this.birthDate = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Adresse e-mail entrée par l'utilisateur.
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Mot de passe entré par l'utilisateur.
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Mot de passe de confirmation entré par l'utilisateur.
        /// </summary>
        public string PasswordConfirmation
        {
            get { return this.passwordConfirmation; }
            set
            {
                if (this.passwordConfirmation != value)
                {
                    this.passwordConfirmation = value;
                    this.RegisterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Est-ce que l'inscription de l'utilisateur est en cours ?
        /// </summary>
        public bool IsRegistering
        {
            get { return this.isRegistering; }
            set
            {
                this.Set(() => this.IsRegistering, ref this.isRegistering, value);
                this.RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande d'inscription.
        /// </summary>
        public RelayCommand RegisterCommand { get; private set; }

        /// <summary>
        ///     Commande de retour à la page précédente.
        /// </summary>
        public RelayCommand GoBackCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les opération qui s'exécute à chaque fois que
        ///     l'utilisateur quitte le formulaire d'inscription.
        /// </summary>
        public override void Cleanup()
        {
            this.FirstName = null;
            this.LastName = null;
            this.Phone = null;
            this.BirthDate = null;
            this.Email = null;
            this.Password = null;
            this.PasswordConfirmation = null;

            base.Cleanup();
        }

        #endregion

        #region Validation

        /// <inheritdoc />
        public override void Validate()
        {
            this.Errors = new List<string>();

            if (!Regex.IsMatch(this.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                this.Errors.Add(this.localizationService.GetString("EmailError"));

            if (!this.PasswordConfirmation.Equals(this.Password))
                this.Errors.Add(this.localizationService.GetString("ConfirmationPasswordError"));

            if (this.Phone?.Length > 20 || this.Phone?.Length < 10)
                this.Errors.Add(this.localizationService.GetString("PhoneError"));
        }

        #endregion

        #region Register

        /// <summary>
        ///     Détermine si la commande d'inscription peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande d'inscription peut s'exécuter ?
        /// </returns>
        private bool CanRegisterExecute()
        {
#pragma warning disable S1067 // L'expression booléen ne devrait pas être si complexe.
            return !this.IsRegistering &&
                   !string.IsNullOrWhiteSpace(this.FirstName) &&
                   !string.IsNullOrWhiteSpace(this.LastName) &&
                   this.BirthDate != null &&
                   !string.IsNullOrWhiteSpace(this.Email) &&
                   !string.IsNullOrWhiteSpace(this.Password) &&
                   !string.IsNullOrWhiteSpace(this.PasswordConfirmation);
#pragma warning restore S1067 // L'expression booléen ne devrait pas être si complexe.
        }

        /// <summary>
        ///     Enregistre un nouvel utilisateur.
        /// </summary>
        private async void Register()
        {
            this.Validate();
            if (!this.IsValid)
            {
                var message = new StringBuilder(this.localizationService.GetString("ValidationError"));
                message.Append(" :")
                       .Append(Environment.NewLine);

                foreach (var error in this.Errors)
                {
                    message.Append(" - ")
                           .Append(error)
                           .Append(Environment.NewLine);
                }

                await this.dialogService.ShowError(message.ToString(), "Hu hu !", "Ok", null);
                return;
            }

            try
            {
                this.IsRegistering = true;

                await this.userService.CreateAsync(new User
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Phone = this.Phone,
                    BirthDate = this.BirthDate.Value.DateTime,
                    Email = this.Email,
                    Password = this.Password
                });

                // Redirection vers la page de connexion.
                this.navigationService.GoBack();
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsRegistering = false;
            }
        }

        #endregion
    }
}
