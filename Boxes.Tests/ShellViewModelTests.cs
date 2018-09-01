using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires qui concernent le <see cref="ShellViewModel"/>.
    /// </summary>
    [TestClass]
    public class ShellViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service de navigation fictif.
        /// </summary>
        private FakeNavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de localization.
        /// </summary>
        private FakeLocalizationService localizationService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de stockage local.
        /// </summary>
        private FakeStorageService storageService;

        /// <summary>
        ///     Stock le view model du shell (ici le view model à tester).
        /// </summary>
        private ShellViewModel shellViewModel;

        #endregion

        #region Tests Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisations qui doivent être exécutées avant chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.storageService = new FakeStorageService();

            this.shellViewModel = new ShellViewModel(this.navigationService, this.localizationService,
                this.storageService);
        }

        /// <summary>
        ///     Effectue les opérations qui doivent être exécutées après chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.navigationService = null;
            this.localizationService = null;
            this.storageService = null;

            this.shellViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur navigue vers la page du shell, le view
        ///     model de cette dernière reçoit les message de type
        ///     <see cref="IsBackButtonVisibleMessage"/>.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToShell_HandlesIsBackButtonVisibleMessage()
        {
            // Arrange
            var user = new User();
            this.shellViewModel.IsBackButtonVisible = false;
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.shellViewModel.Initialize();
            Messenger.Default.Send(new IsBackButtonVisibleMessage());

            // Assert
            Assert.IsTrue(this.shellViewModel.IsBackButtonVisible);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur navigue vers la page du shell, le view
        ///     model de cette dernière reçoit les message de type
        ///     <see cref="ShellTitleMessage"/>.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToShell_HandlesShellTitleMessage()
        {
            // Arrange
            var user = new User();
            this.shellViewModel.Title = null;
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.shellViewModel.Initialize();
            Messenger.Default.Send(new ShellTitleMessage("Test"));

            // Assert
            Assert.AreEqual("Test", this.shellViewModel.Title);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur navigue vers la page du shell, la
        ///     propriété <c>CurrentUserName</c> du view model de cette dernière
        ///     est bien le nom et prénom de l'utilisateur connecté sur Boxes.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToShell_CurrentUserNameLikeInLocalSettings()
        {
            // Arrange
            var user = new User { FirstName = "John", LastName = "Doe" };
            this.shellViewModel.CurrentUserName = null;
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.shellViewModel.Initialize();

            // Assert
            Assert.AreEqual(user.ToString(), this.shellViewModel.CurrentUserName);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur navigue vers la page du shell, la
        ///     page actuellement affichée (dans le shell) est la page d'accueil.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToShell_CurrentPageIsHome()
        {
            // Arrange
            var user = new User();
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.Initialize();

            // Assert
            Assert.AreEqual("Home", this.navigationService.CurrentPageKey);
        }

        #endregion

        #region Navigation commands

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur veut retourner en arrière et qu'il
        ///     fait appel à la commande de retour arrière, la page actuellement affichée
        ///     est "RootPage".
        /// </summary>
        [TestMethod]
        public void GoBackCommand_BackToPreviousPage_CurrentPageIsRootPage()
        {
            // Arrange
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.GoBackCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur clique sur le bouton pour aller sur
        ///     la page d'accueil, la page actuellement affichée (dans le shell) est
        ///     bien celle-ci.
        /// </summary>
        [TestMethod]
        public void NavigateToHomeCommand_GoToHomePage_CurrentPageIsHome()
        {
            // Arrange
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.NavigateToHomeCommand.Execute(null);

            // Assert
            Assert.AreEqual("Home", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur clique sur le bouton pour aller sur
        ///     la page "Découvrir", la page actuellement affichée (dans le shell) est
        ///     bien celle-ci.
        /// </summary>
        [TestMethod]
        public void NavigateToDiscoverCommand_GoToDiscoverPage_CurrentPageIsDiscover()
        {
            // Arrange
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.NavigateToDiscoverCommand.Execute(null);

            // Assert
            Assert.AreEqual("Discover", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur clique sur le bouton pour aller sur
        ///     la page "Mes boites", la page actuellement affichée (dans le shell)
        ///     est bien celle-ci.
        /// </summary>
        [TestMethod]
        public void NavigateToMyBoxesCommand_GoToMyBoxesPage_CurrentPageIsMyBoxes()
        {
            // Arrange
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.NavigateToMyBoxesCommand.Execute(null);

            // Assert
            Assert.AreEqual("MyBoxes", this.navigationService.CurrentPageKey);
        }

        #endregion

        #region Signout command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur se déconnecte, ce dernier n'est plus
        ///     dans le conteneur de paramètres d'application.
        /// </summary>
        [TestMethod]
        public void SignoutCommand_UserSignout_UserNotInLocalSettings()
        {
            // Arrange
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.shellViewModel.SignoutCommand.Execute(null);

            // Assert
            Assert.IsFalse(this.storageService.LocalSettings.ContainsKey("CurrentUser"));
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur se déconnecte, la page actuellement
        ///     affichée est la page de connexion.
        /// </summary>
        [TestMethod]
        public void SignoutCommand_UserSignout_CurrentPageIsLogin()
        {
            // Arrange
            this.navigationService.NavigateTo("Shell");

            // Act
            this.shellViewModel.SignoutCommand.Execute(null);

            // Assert
            Assert.AreEqual("Login", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
