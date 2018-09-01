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
    ///     Effectue les tests unitaires relatifs au view model <see cref="HomeViewModel"/>.
    /// </summary>
    [TestClass]
    public class HomeViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de stockage local.
        /// </summary>
        private FakeStorageService storageService;

        /// <summary>
        ///     Stock le service de navigation fictive.
        /// </summary>
        private FakeNavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Post"/>.
        /// </summary>
        private FakePostService postService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de localization.
        /// </summary>
        private FakeLocalizationService localizationService;

        /// <summary>
        ///     Stock le service fictif d'affichage de popups.
        /// </summary>
        private FakeDialogService dialogService;

        /// <summary>
        ///     Stock le view model de la page d'accueil (ici le view model à tester).
        /// </summary>
        private HomeViewModel homeViewModel;

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
            this.storageService = new FakeStorageService();
            this.navigationService = new FakeNavigationService();
            this.postService = new FakePostService();
            this.localizationService = new FakeLocalizationService();
            this.dialogService = new FakeDialogService();

            this.homeViewModel = new HomeViewModel(this.storageService, this.navigationService,
                this.postService, this.localizationService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.storageService = null;
            this.navigationService = null;
            this.postService = null;
            this.localizationService = null;
            this.dialogService = null;

            this.homeViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'accueil,
        ///     la propriété <c>Posts</c> du view model de cette page ne soit
        ///     pas une collection vide.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task Initialize_NavigationToHome_PostsNotEmpty()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var post = new Post
            {
                Id = random.Next(50, 100),
                Box = new Box { Creator = user }
            };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));
            await this.postService.CreateAsync(post);

            // Act
            this.homeViewModel.Initialize();

            // Assert
            Assert.AreEqual(1, this.postService.Posts.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'accuei,
        ///     un message de type <see cref="ShellTitleMessage"/> est envoyé
        ///     afin de demander le changement du titre du shell.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToHome_ShellTitleMessageSent()
        {
            // Arrange
            var wasShellMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellMessageSent = true);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.homeViewModel.Initialize();

            // Assert
            Assert.IsTrue(wasShellMessageSent);
        }

        #endregion

        #region Show post command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur veut voir le détail d'un post et
        ///     appel ainsi la commande d'affichage d'un post, la page actuellement
        ///     affichée est celle de ce post.
        /// </summary>
        [TestMethod]
        public void ShowPostCommand_GoToPostPage_CurrentPageIsPost()
        {
            // Arrange
            var post = new Post();
            this.navigationService.NavigateTo("Home");

            // Act
            this.homeViewModel.ShowPostCommand.Execute(post);

            // Assert
            Assert.AreEqual("Post", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur veut voir le détail d'un post et
        ///     appel ainsi la commande d'affichage d'un post, le paramètre de la
        ///     page actuellement affichée est bien le post à afficher.
        /// </summary>
        [TestMethod]
        public void ShowPostCommand_GoToPostPage_CurrentPageParameterIsPostToShow()
        {
            // Arrange
            var post = new Post { Id = random.Next(50) };

            // Act
            this.homeViewModel.ShowPostCommand.Execute(post);

            // Assert
            Assert.IsInstanceOfType(this.navigationService.CurrentPageParameter, typeof(Post));
            Assert.AreEqual(post.Id, (this.navigationService.CurrentPageParameter as Post).Id);
        }

        #endregion
    }
}
