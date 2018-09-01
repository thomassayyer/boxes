using Boxes.Models;
using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires qui concernent le <see cref="LoginViewModel"/>.
    /// </summary>
    [TestClass]
    public class LoginViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Models.User"/>.
        /// </summary>
        private FakeUserService userService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de stockage local.
        /// </summary>
        private FakeStorageService storageService;

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
        ///     Stock le view model de la page de connexion (le view model à tester).
        /// </summary>
        private LoginViewModel loginViewModel;

        /// <summary>
        ///     Stock l'objet de génération de nombres aléatoires.
        /// </summary>
        private static Random random;

        #endregion

        #region Class Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation qui doivent être faites avant l'exécution du
        ///     premier test.
        /// </summary>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            random = new Random();
        }

        /// <summary>
        ///     Effectue les nettoyages qui doivent être faits après l'exécution du
        ///     dernier test.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            random = null;
        }

        #endregion

        #region Tests Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation qui doivent être exécutées avant l'exécution
        ///     de chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.userService = new FakeUserService();
            this.storageService = new FakeStorageService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.loginViewModel = new LoginViewModel(this.userService, this.storageService,
                this.navigationService, this.localizationService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanUp()
        {
            this.userService = null;
            this.storageService = null;
            this.navigationService = null;
            this.localizationService = null;
            this.dialogService = null;

            this.loginViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur effectue une navigation à partir de la
        ///     page de connexion, le champ "Adresse email" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromLogin_EmailIsNull()
        {
            // Arrange
            this.loginViewModel.Email = "john.doe@example.com";

            // Act
            this.loginViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.loginViewModel.Email);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur effectue une navigation à partir de la
        ///     page de connexion, le champ "Mot de passe" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromLogin_PasswordIsNull()
        {
            // Arrange
            this.loginViewModel.Password = "johndoe";

            // Act
            this.loginViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.loginViewModel.Password);
        }

        #endregion

        #region Login command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas d'adresse email, la
        ///     commande de connexion ne peut s'exécuter.
        /// </summary>
        /// <param name="email">
        ///     Email à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void LoginCommand_EmailIsNullOrWhiteSpace_CannotExecute(string email)
        {
            // Arrange
            this.loginViewModel.Email = email;
            this.loginViewModel.Password = "johndoe";
            this.loginViewModel.IsLoggingIn = false;

            // Act
            var canLoginExecute = this.loginViewModel.LoginCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canLoginExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur n'entre pas de mot de passe, la
        ///     commande de connexion ne peut s'exécuter.
        /// </summary>
        /// <param name="password">
        ///     Mot de passe à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void LoginCommand_PasswordIsNullOrWhiteSpace_CannotExecute(string password)
        {
            // Arrange
            this.loginViewModel.Email = "john.doe@example.com";
            this.loginViewModel.Password = password;
            this.loginViewModel.IsLoggingIn = false;

            // Act
            var canLoginExecute = this.loginViewModel.LoginCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canLoginExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la connexion de l'utilisateur est en cours, la
        ///     commande de connexion ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void LoginCommand_IsLoggingIn_CannotExecute()
        {
            // Arrange
            this.loginViewModel.Email = "john.doe@example.com";
            this.loginViewModel.Password = "johndoe";
            this.loginViewModel.IsLoggingIn = true;

            // Act
            var canLoginExecute = this.loginViewModel.LoginCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canLoginExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la connexion de l'utilisateur n'est pas en cours
        ///     et que ce dernier a entré une adresse email et un mot de passe, la
        ///     commande de connexion peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void LoginCommand_IsNotLoggingInEmailPasswordNotNullOrWhiteSpace_CanExecute()
        {
            // Arrange
            this.loginViewModel.Email = "john.doe@example.com";
            this.loginViewModel.Password = "johndoe";
            this.loginViewModel.IsLoggingIn = false;

            // Act
            var canLoginExecute = this.loginViewModel.LoginCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canLoginExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre une adresse email qui existe
        ///     mais un mot de passe incorrect, la page actuellement affichée est
        ///     toujours la page de connexion.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task LoginCommand_CorrectEmailWrongPassword_CurrentPageIsLogin()
        {
            // Arrange
            var user = new User
            {
                Email = "john.doe@example.com",
                Password = "johndoe"
            };
            this.loginViewModel.Email = user.Email;
            this.loginViewModel.Password = "wrongpass";
            await this.userService.CreateAsync(user);
            this.navigationService.NavigateTo("Login");

            // Act
            this.loginViewModel.LoginCommand.Execute(null);

            // Assert
            Assert.AreEqual("Login", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre une adresse email qui existe
        ///     mais un mot de passe incorrect, aucun utilisateur n'est inscrit dans
        ///     le conteneur de paramètre d'application.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task LoginCommand_CorrectEmailWrongPassword_UserNotInLocalSettings()
        {
            // Arrange
            var user = new User
            {
                Email = "john.doe@example.com",
                Password = "johndoe"
            };
            this.loginViewModel.Email = user.Email;
            this.loginViewModel.Password = "wrongpass";
            await this.userService.CreateAsync(user);

            // Act
            this.loginViewModel.LoginCommand.Execute(null);

            // Assert
            Assert.IsFalse(this.storageService.LocalSettings.ContainsKey("CurrentUser"));
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre une adresse email qui existe
        ///     mais un mot de passe incorrect, un message d'erreur est affiché dans
        ///     une popup.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task LoginCommand_CorrectEmailWrongPassword_NotificationErrorShown()
        {
            // Arrange
            var user = new User
            {
                Email = "john.doe@example.com",
                Password = "johndoe"
            };
            this.loginViewModel.Email = user.Email;
            this.loginViewModel.Password = "wrongpass";
            await this.userService.CreateAsync(user);


            // Act
            this.loginViewModel.LoginCommand.Execute(null);

            // Assert
            Assert.AreEqual("ShowError", this.dialogService.MethodCalled);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre une adresse email qui existe
        ///     et le mot de passe correspondant, celui-ci est inscrit dans le
        ///     conteneur de paramètres d'application.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task LoginCommand_CorrectEmailCorrectPassword_UserInLocalSettings()
        {
            // Arrange
            var user = new User
            {
                Email = "john.doe@example.com",
                Password = "johndoe"
            };
            this.loginViewModel.Email = user.Email;
            this.loginViewModel.Password = user.Password;
            await this.userService.CreateAsync(user);

            // Act
            this.loginViewModel.LoginCommand.Execute(null);

            // Assert
            Assert.IsTrue(this.storageService.LocalSettings.ContainsKey("CurrentUser"));
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre une adresse email qui existe
        ///     et le mot de passe correspondant, la page actuellement affichée est
        ///     la page "coquille" (ou "shell").
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task LoginCommand_CorrectEmailCorrectPassword_CurrentPageIsShell()
        {
            // Arrange
            var user = new User
            {
                Email = "john.doe@example.com",
                Password = "johndoe"
            };
            this.loginViewModel.Email = user.Email;
            this.loginViewModel.Password = user.Password;
            await this.userService.CreateAsync(user);
            this.navigationService.NavigateTo("Login");

            // Act
            this.loginViewModel.LoginCommand.Execute(null);

            // Assert
            Assert.AreEqual("Shell", this.navigationService.CurrentPageKey);
        }

        #endregion

        #region Register command

        /// <summary>
        ///     Vérifie que lorsque la commande d'inscription est exécutée, la
        ///     page actuellement affichée est la page d'inscription.
        /// </summary>
        [TestMethod]
        public void RegisterCommand_UserNotSignup_CurrentPageIsRegister()
        {
            // Arrange
            this.navigationService.NavigateTo("Login");

            // Act
            this.loginViewModel.RegisterCommand.Execute(null);

            // Assert
            Assert.AreEqual("Register", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
