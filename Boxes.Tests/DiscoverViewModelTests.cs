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
    ///     Effectue les tests unitaires du view model <see cref="DiscoverViewModel"/>.
    /// </summary>
    [TestClass]
    public class DiscoverViewModelTests
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
        ///     Stock le view model de la page "Découvrir" (ici le view model à tester).
        /// </summary>
        private DiscoverViewModel discoverViewModel;

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
        ///     Effectue les initialisation qui s'exécute avant chaque test.
        /// </summary>
        [TestInitialize]
        public void TestsInitialize()
        {
            this.boxService = new FakeBoxService();
            this.navigationService = new FakeNavigationService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.discoverViewModel = new DiscoverViewModel(this.boxService, this.navigationService,
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

            this.discoverViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la liste du top
        ///     des boites n'est pas vide.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task Initialize_NavigationToDiscover_TopBoxesNotEmpty()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };
            await this.boxService.CreateAsync(box);

            // Act
            this.discoverViewModel.Initialize();

            // Assert
            Assert.AreEqual(1, this.discoverViewModel.TopBoxes.Count);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de
        ///     type <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToDiscover_ShellTitleMessageSent()
        {
            // Arrange
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellTitleMessageSent = true);

            // Act
            this.discoverViewModel.Initialize();

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        #endregion

        #region Show box command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur fait appel à la commande d'affichage
        ///     d'une boite, la page actuellement affichée est la page d'une boite.
        /// </summary>
        [TestMethod]
        public void ShowBoxCommand_GoToBoxPage_CurrentPageIsBox()
        {
            // Arrange
            var box = new Box();
            this.navigationService.NavigateTo("Discover");

            // Act
            this.discoverViewModel.ShowBoxCommand.Execute(box);

            // Assert
            Assert.AreEqual("Box", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur fait appel à la commande d'affichage
        ///     d'une boite, le paramètre transmis à la page actuellement affichée est
        ///     du type <see cref="Box"/>.
        /// </summary>
        [TestMethod]
        public void ShowBoxCommand_GoToBoxPage_CurrentPageParameterIsBoxToShow()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };

            // Act
            this.discoverViewModel.ShowBoxCommand.Execute(box);

            // Assert
            Assert.IsInstanceOfType(this.navigationService.CurrentPageParameter, typeof(Box));
            Assert.AreEqual(box.Id, (this.navigationService.CurrentPageParameter as Box).Id);
        }

        #endregion

        #region Search box command

        /// <summary>
        ///     Vérifie que lorsque la commande de recherche d'une boite est appelée
        ///     mais que les termes de la recherche sont trop courts (inférieurs ou
        ///     égales à 2 caractères), le mode de recherche (attribut
        ///     <c>IsSearching</c>)^n'est pas actif.
        /// </summary>
        [TestMethod]
        public void SearchBoxCommand_TermsTooShort_IsNotSearching()
        {
            // Arrange
            var terms = new string('*', 2);

            // Act
            this.discoverViewModel.SearchBoxCommand.Execute(terms);

            // Assert
            Assert.IsFalse(this.discoverViewModel.IsSearching);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de recherche d'une boite est appelée
        ///     mais que les termes de la recherche sont trop courts (inférieurs ou
        ///     égales à 2 caractères), la liste des résultats de la recherche est
        ///     vide.
        /// </summary>
        [TestMethod]
        public void SearchBoxCommand_TermsTooShort_SearchResultsEmpty()
        {
            // Arrange
            var terms = new string('*', 2);

            // Act
            this.discoverViewModel.SearchBoxCommand.Execute(terms);

            // Assert
            Assert.AreEqual(0, this.discoverViewModel.SearchResults.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de recherche d'une boite est appelée
        ///     et que les termes de la recherches sont valides (supérieurs à 2
        ///     caractères), les résultats de recherches ne sont pas vides.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task SearchBoxCommand_ValidTerms_SearchResultsNotEmpty()
        {
            // Arrange
            var terms = "Lorem";
            var box = new Box
            {
                Title = terms,
                Description = "Dolor sit amet"
            };
            await this.boxService.CreateAsync(box);

            // Act
            this.discoverViewModel.SearchBoxCommand.Execute(terms);

            // Assert
            Assert.AreEqual(1, this.discoverViewModel.SearchResults.Count);
        }

        #endregion
    }
}
