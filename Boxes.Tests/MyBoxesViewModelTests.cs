using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires qui concernent le view model <see cref="MyBoxesViewModel"/>.
    /// </summary>
    [TestClass]
    public class MyBoxesViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Box"/>.
        /// </summary>
        private FakeBoxService boxService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives du stockage local.
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
        ///     Stock le view model de la page "Mes boites" (ici le view model à tester).
        /// </summary>
        private MyBoxesViewModel myBoxesViewModel;

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
            this.boxService = new FakeBoxService();
            this.storageService = new FakeStorageService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.myBoxesViewModel = new MyBoxesViewModel(this.boxService, this.storageService,
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

            this.myBoxesViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la liste des boites
        ///     de l'utilisateur courant n'est pas vide.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task Initialize_NavigationToMyBoxes_BoxesNotEmpty()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box { Creator = user };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));
            await this.boxService.CreateAsync(box);

            // Act
            this.myBoxesViewModel.Initialize();

            // Assert
            Assert.AreEqual(1, this.myBoxesViewModel.Boxes.Count);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de type
        ///     <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToMyBoxes_ShellTitleMessageSent()
        {
            // Arrange
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellTitleMessageSent = true);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.myBoxesViewModel.Initialize();

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        #endregion

        #region Show box command

        /// <summary>
        ///     Vérifie que lorsque la commande d'affichage d'une boite est appelée,
        ///     la page actuellement affichée est bien celle d'une boite.
        /// </summary>
        [TestMethod]
        public void ShowBoxCommand_GoToBoxPage_CurrentPageIsBox()
        {
            // Arrange
            var box = new Box();
            this.navigationService.NavigateTo("MyBoxes");

            // Act
            this.myBoxesViewModel.ShowBoxCommand.Execute(box);

            // Assert
            Assert.AreEqual("Box", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande d'affichage d'une boite est appelée,
        ///     le paramètre transmis à la page actuellement affichée est bien la
        ///     boite à afficher.
        /// </summary>
        [TestMethod]
        public void ShowBoxCommand_GoToBoxPage_CurrentPageParameterIsBoxToShow()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };

            // Act
            this.myBoxesViewModel.ShowBoxCommand.Execute(box);

            // Assert
            Assert.IsInstanceOfType(this.navigationService.CurrentPageParameter, typeof(Box));
            Assert.AreEqual(box.Id, (this.navigationService.CurrentPageParameter as Box).Id);
        }

        #endregion

        #region Show create box command

        /// <summary>
        ///     Vérifie que lorsque la commande d'affichage de la page de création
        ///     d'une boite est appelée, la page actuellement affichée est bien
        ///     celle-ci.
        /// </summary>
        [TestMethod]
        public void ShowCreateBoxCommand_GoToCreateBox_CurrentPageIsCreateBox()
        {
            // Arrange
            this.navigationService.NavigateTo("MyBoxes");

            // Act
            this.myBoxesViewModel.ShowCreateBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual("CreateBox", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
