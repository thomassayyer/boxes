using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires qui concernent le view model <see cref="EditBoxViewModel"/>.
    /// </summary>
    [TestClass]
    public class EditBoxViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Box"/>.
        /// </summary>
        private FakeBoxService boxService;

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
        ///     Stock le view model de la page d'édition d'une boite (ici le view model à
        ///     tester).
        /// </summary>
        private EditBoxViewModel editBoxViewModel;

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
        ///     Effectue les initialisation qui doivent s'exécuter avant chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.boxService = new FakeBoxService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.editBoxViewModel = new EditBoxViewModel(this.boxService, this.navigationService,
                this.localizationService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.boxService = null;
            this.navigationService = null;
            this.localizationService = null;
            this.dialogService = null;

            this.editBoxViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété <c>Id</c>
        ///     du view model est l'id de la boite transmise.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_IdIsBoxId()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };

            // Act
            this.editBoxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Id, this.editBoxViewModel.Id);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Title</c> est le titre de la boite transmise.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_TitleIsBoxTitle()
        {
            // Arrange
            var box = new Box { Title = "Lorem" };

            // Act
            this.editBoxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Title, this.editBoxViewModel.Title);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Description</c> est la description de la boite transmise.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_DescriptionIsBoxDescription()
        {
            // Arrange
            var box = new Box { Description = "Lorem ipsum dolor sit amet" };

            // Act
            this.editBoxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Description, this.editBoxViewModel.Description);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de type
        ///     <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToEditBox_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var box = new Box();
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.editBoxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de type
        ///     <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToEditBox_ShellTitleMessageSent()
        {
            // Arrange
            var box = new Box();
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellTitleMessageSent = true);

            // Act
            this.editBoxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'édition d'une boite, le
        ///     champ "Titre" du formulaire est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromEditBox_TitleIsNull()
        {
            // Arrange
            this.editBoxViewModel.Title = "Lorem";

            // Act
            this.editBoxViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.editBoxViewModel.Title);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'édition d'une boite, le
        ///     champ "Description" du formulaire est bien vidé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromEditBox_DescriptionIsNull()
        {
            // Arrange
            this.editBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            this.editBoxViewModel.Cleanup();

            // Assert
            Assert.IsNull(this.editBoxViewModel.Description);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'édition d'une boite, un
        ///     message de type <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromBox_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.editBoxViewModel.Cleanup();

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        #endregion

        #region Update box command

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si une boite est en cours d'édition,
        ///     la commande ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void UpdateBoxCommand_IsUpdating_CannotExecute()
        {
            // Arrange
            this.editBoxViewModel.IsUpdating = true;
            this.editBoxViewModel.Title = "Lorem";
            this.editBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canUpdateBoxExecute = this.editBoxViewModel.UpdateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canUpdateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si aucun titre n'a été entré (ou
        ///     succession d'espaces), la commande ne peut s'exécuter.
        /// </summary>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void UpdateBoxCommand_TitleIsNullOrWhiteSpace_CannotExecute(string title)
        {
            // Arrange
            this.editBoxViewModel.IsUpdating = false;
            this.editBoxViewModel.Title = title;
            this.editBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canEditBoxExecute = this.editBoxViewModel.UpdateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canEditBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si aucune description n'a été
        ///     entrée (ou succession d'espaces), la commande ne peut s'exécuter.
        /// </summary>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void UpdateBoxCommand_DescriptionIsNullOrWhiteSpace_CannotExecute(string description)
        {
            // Arrange
            this.editBoxViewModel.IsUpdating = false;
            this.editBoxViewModel.Title = "Lorem";
            this.editBoxViewModel.Description = description;

            // Act
            bool canUpdateBoxExecute = this.editBoxViewModel.UpdateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canUpdateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si aucune boite n'est en cours de
        ///     modification et que les champs requis sont remplis, la commande peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void UpdateBoxCommand_IsNotUpdatingRequiredFieldsNotWhiteSpace_CanExecute()
        {
            // Arrange
            this.editBoxViewModel.IsUpdating = false;
            this.editBoxViewModel.Title = "Lorem";
            this.editBoxViewModel.Description = "Lorem ipsum dolor sit amet";

            // Act
            bool canUpdateBoxExecute = this.editBoxViewModel.UpdateBoxCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canUpdateBoxExecute);
        }

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si les données du formulaire
        ///     sont valides, une boite a été modifiée.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task UpdateBoxCommand_ValidData_BoxUpdated()
        {
            // Arrange
            var box = new Box
            {
                Id = random.Next(50),
                Title = "Lorem",
                Description = "Lorem ipsum dolor sit amet",
            };
            await this.boxService.CreateAsync(box);
            this.editBoxViewModel.Id = box.Id;
            this.editBoxViewModel.Description = "Lorem test";
            this.editBoxViewModel.Title = box.Title;

            // Act
            this.editBoxViewModel.UpdateBoxCommand.Execute(null);

            // Assert
            Assert.IsNotNull(this.boxService.Boxes.Find(
                b => b.Description == this.editBoxViewModel.Description));
        }

        /// <summary>
        ///     Vérifie que lors de la modification d'une boite si les données du formulaire
        ///     sont valides, la page actuellement affichée est la page précédente.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task UpdateBoxCommand_ValidData_CurrentPageIsRootPage()
        {
            // Arrange
            this.navigationService.NavigateTo("EditBox");
            var box = new Box
            {
                Id = random.Next(50),
                Title = "Lorem",
                Description = "Lorem ipsum dolor sit amet",
            };
            await this.boxService.CreateAsync(box);
            this.editBoxViewModel.Id = box.Id;
            this.editBoxViewModel.Description = "Lorem test";
            this.editBoxViewModel.Title = box.Title;

            // Act
            this.editBoxViewModel.UpdateBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
