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
    ///     Effectue les tests unitaires concernant le view model <see cref="CreateBoxViewModel"/>.
    /// </summary>
    [TestClass]
    public class CreateBoxViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Box"/>.
        /// </summary>
        private FakeBoxService boxService;

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
        ///     Stock le view model de la page de création d'une boite (ici le view model
        ///     à tester).
        /// </summary>
        private CreateBoxViewModel createBoxViewModel;

        #endregion

        #region Tests Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation qui doivent s'exécuter avant chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.boxService = new FakeBoxService();
            this.storageService = new FakeStorageService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.createBoxViewModel = new CreateBoxViewModel(this.boxService, this.storageService,
                this.navigationService, this.localizationService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.boxService = null;
            this.storageService = null;
            this.navigationService = null;
            this.localizationService = null;
            this.dialogService = null;

            this.createBoxViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie si lors de l'initialisation du view model, un message de type
        ///     <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToCreateBox_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.createBoxViewModel.Initialize();

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        /// <summary>
        ///     Vérifie si lors de l'initialisation du view model, un message de type
        ///     <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToCreateBox_ShellTitleMessageSent()
        {
            // Arrange
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellTitleMessageSent = true);

            // Act
            this.createBoxViewModel.Initialize();

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        /// <summary>
        ///     Vérifie si lorsque l'utilisateur quitte la page de création d'une boite,
        ///     un message de type <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromCreateBox_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            Messenger.Reset();
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.createBoxViewModel.Cleanup();

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page de création d'une boite,
        ///     le champ "Titre" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromCreateBox_TitleIsNull()
        {
            // Arrange
            this.createBoxViewModel.Title = "Lorem";

            // Act
            this.createBoxViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.createBoxViewModel.Title);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page de création d'une boite,
        ///     le champ "Description" est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromCreateBox_DescriptionIsNull()
        {
            // Arrange
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            this.createBoxViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.createBoxViewModel.Description);
        }

        #endregion

        #region Create box command

        /// <summary>
        ///     Vérifie que lors de la création d'une boite si une boite est déjà en cours de
        ///     création, la commande ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreateBoxCommand_IsCreating_CannotExecute()
        {
            // Arrange
            this.createBoxViewModel.IsCreating = true;
            this.createBoxViewModel.Title = "Lorem";
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canCreateBoxExecute = this.createBoxViewModel.CreateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCreateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la création d'une boite si le titre de la boite n'a pas
        ///     été entré (ou succession d'espaces), la commande ne peut s'exécuter.
        /// </summary>
        /// <param name="title">
        ///     Titre auto-généré pour le test (null, "" ou " ").
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void CreateBoxCommand_TitleIsNullOrWhiteSpace_CannotExecute(string title)
        {
            // Arrange
            this.createBoxViewModel.IsCreating = false;
            this.createBoxViewModel.Title = title;
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canCreateBoxExecute = this.createBoxViewModel.CreateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCreateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la création d'une boite si la description n'a pas été
        ///     entrée (ou succession d'espaces), la commande ne peut s'exécuter.
        /// </summary>
        /// <param name="description">
        ///     Description auto-généré pour le test (null, "" ou " ").
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void CreateBoxCommand_DescriptionIsNullOrWhiteSpace_CannotExecute(string description)
        {
            // Arrange
            this.createBoxViewModel.IsCreating = false;
            this.createBoxViewModel.Title = "Lorem";
            this.createBoxViewModel.Description = description;

            // Act
            bool canCreateBoxExecute = this.createBoxViewModel.CreateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canCreateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la création d'une boite si aucune boite n'est en cours de
        ///     création et que les champs obligatoires sont remplis, la commande peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreateBoxCommand_IsNotCreatingRequiredFieldNotNullOrWhiteSpace_CanExecute()
        {
            // Arrange
            this.createBoxViewModel.IsCreating = false;
            this.createBoxViewModel.Title = "Lorem";
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canCreateBoxExecute = this.createBoxViewModel.CreateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canCreateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la création d'une boite avec des données valides,
        ///     une boite est bien créée.
        /// </summary>
        [TestMethod]
        public void CreateBoxCommand_ValidData_BoxCreated()
        {
            // Arrange
            this.createBoxViewModel.Title = "Lorem";
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.createBoxViewModel.CreateBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, this.boxService.Boxes.Count);
        }

        /// <summary>
        ///     Vérifie que lors de la création d'une boite avec des données valides,
        ///     la page actuellement affichée est la page précédente.
        /// </summary>
        [TestMethod]
        public void CreateBoxCommand_ValidData_CurrentPageIsRootPage()
        {
            // Arrange
            this.createBoxViewModel.Title = "Lorem";
            this.createBoxViewModel.Description = "Lorem ipsum dolor sit amet";
            this.navigationService.NavigateTo("CreateBox");
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.createBoxViewModel.CreateBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
