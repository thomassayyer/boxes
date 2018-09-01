using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires qui concernent le <see cref="RegisterViewModel"/>.
    /// </summary>
    [TestClass]
    public class RegisterViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Models.User"/>.
        /// </summary>
        private FakeUserService userService;

        /// <summary>
        ///     Stock le service de navigation fictif.
        /// </summary>
        private FakeNavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de localization.
        /// </summary>
        private FakeLocalizationService localizationService;

        /// <summary>
        ///     Stock le service fictif d'affichage de popups.
        /// </summary>
        private FakeDialogService dialogService;

        /// <summary>
        ///     Stock le view model de la page d'inscription (ici le view model à tester).
        /// </summary>
        private RegisterViewModel registerViewModel;

        #endregion

        #region Tests Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisations qui doivent être exécutées avant
        ///     chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.userService = new FakeUserService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.registerViewModel = new RegisterViewModel(this.userService, this.navigationService,
                this.localizationService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.userService = null;
            this.navigationService = null;
            this.localizationService = null;
            this.dialogService = null;

            this.registerViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Prénom" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_FirstNameIsNull()
        {
            // Arrange
            this.registerViewModel.FirstName = "John";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.FirstName);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Nom" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_LastNameIsNull()
        {
            // Arrange
            this.registerViewModel.LastName = "Doe";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.LastName);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Numéro de tél." est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_PhoneIsNull()
        {
            // Arrange
            this.registerViewModel.Phone = "0779828612";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.Phone);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Date de naissance" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_BirthDateIsNull()
        {
            // Arrange
            this.registerViewModel.BirthDate = DateTime.Now;

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.BirthDate);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Adresse email" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_EmailIsNull()
        {
            // Arrange
            this.registerViewModel.Email = "john.doe@gmail.com";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.Email);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Mot de passe" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_PasswordIsNull()
        {
            // Arrange
            this.registerViewModel.Password = "johndoe";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.Password);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription le champ
        ///     "Confirmation" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromRegister_PasswordConfirmationIsNull()
        {
            // Arrange
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            this.registerViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.registerViewModel.PasswordConfirmation);
        }

        #endregion

        #region Register command

        /// <summary>
        ///     Vérifie que lorsque l'inscription est en cours, la commande d'inscription
        ///     ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_IsRegistering_CannotExecute()
        {
            // Arrange
            this.registerViewModel.IsRegistering = true;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            var canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de prénom, la commande d'inscription
        ///     ne peut s'exécuter.
        /// </summary>
        /// <param name="firstName">
        ///     Prénom à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterCommand_FirstNameIsNullOrWhiteSpace_CannotExecute(string firstName)
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = firstName;
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de nom, la commande d'inscription
        ///     ne peut s'exécuter.
        /// </summary>
        /// <param name="lastName">
        ///     Nom à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterCommand_LastNameIsNullOrWhiteSpace_CannotExecute(string lastName)
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = lastName;
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de date de naissance, la commande
        ///     d'inscription ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_BirthDateIsNull_CannotExecute()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = null;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas d'adresse email, la commande
        ///     d'inscription ne peut s'exécuter.
        /// </summary>
        /// <param name="email">
        ///     Email à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterCommand_EmailIsNullOrWhiteSpace_CannotExecute(string email)
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = email;
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de mot de passe, la commande
        ///     d'inscription ne peut s'exécuter.
        /// </summary>
        /// <param name="password">
        ///     Mot de passe à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterCommand_PasswordIsNullOrWhiteSpace_CannotExecute(string password)
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = password;
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de mo de passe de confirmation,
        ///     la commande d'inscription ne peut s'exécuter.
        /// </summary>
        /// <param name="confirmation">
        ///     Mot de passe de confirmation à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterCommand_PasswordConfirmationIsNullOrWhiteSpace_CannotExecute(string confirmation)
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = confirmation;

            // Act
            bool canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'inscription n'est pas en cours, la commande
        ///     d'inscription peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_IsNotRegisteringRequiredFieldsNotNullOrWhiteSpace_CanExecute()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            var canRegisterExecute = this.registerViewModel.RegisterCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canRegisterExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre un numéro de téléphone trop long
        ///     (c-à-d. suppérieur à 20 chiffres) un message d'erreur est affiché dans
        ///     une popup.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_PhoneLengthTooLong_ErrorNotificationShown()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";
            this.registerViewModel.Phone = new string('*', 21);

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("ShowError", this.dialogService.MethodCalled);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre un numéro de téléphone trop court
        ///     (c-à-d. inférieur à 10 chiffres) un message d'erreur est affiché dans
        ///     une popup.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_PhoneLengthTooShort_ErrorNotificationShown()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";
            this.registerViewModel.Phone = new string('*', 9);

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("ShowError", this.dialogService.MethodCalled);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas une adresse email valide, un
        ///     message d'erreur est affiché dans une popup.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_EmailIsNotAnEmail_ErrorNotificationShown()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("ShowError", this.dialogService.MethodCalled);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre des mots de passe qui ne correspondent
        ///     pas, un message d'erreur est affiché dans une popup.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_PasswordsNotCorresponding_ErrorNotificationShown()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johnd";

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("ShowError", this.dialogService.MethodCalled);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre des valeurs qui ne correspondent pas
        ///     aux règles de saisies définies, aucun utilisateur n'est créé.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_InvalidData_UserNotCreated()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual(0, this.userService.Users.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre des valeurs qui ne correspondent pas
        ///     aux règles de saisies définies, la page actuellement affichée est toujours
        ///     la page d'inscription.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_InvalidData_CurrentPageIsRegister()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";
            this.navigationService.NavigateTo("Register");

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("Register", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre des valeurs qui correspondent aux
        ///     règles de saisies définies, ce dernier est bien créé.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_ValidData_UserCreated()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, this.userService.Users.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre des valeurs qui correspondent aux
        ///     règles de saisies définies, la page actuellement affichée est la page
        ///     précédente.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_ValidData_CurrentPageIsRootPage()
        {
            // Arrange
            this.registerViewModel.IsRegistering = false;
            this.registerViewModel.FirstName = "John";
            this.registerViewModel.LastName = "Doe";
            this.registerViewModel.BirthDate = DateTime.Now;
            this.registerViewModel.Email = "john.doe@gmail.com";
            this.registerViewModel.Password = "johndoe";
            this.registerViewModel.PasswordConfirmation = "johndoe";
            this.navigationService.NavigateTo("Register");

            // Act
            this.registerViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        #endregion

        #region Go back command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'inscription, la page
        ///     actuellement affichée est "RootPage".
        /// </summary>
        [TestMethod]
        public void GoBackCommand_LeavingRegisterPage_CurrentPageIsRootPage()
        {
            // Arrange
            this.navigationService.NavigateTo("Register");

            // Act
            this.registerViewModel.GoBackCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
