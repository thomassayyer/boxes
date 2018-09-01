using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Tests.Mock.Services;
using Boxes.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Tests
{
    /// <summary>
    ///     Effectue les tests unitaires du view model <see cref="BoxViewModel"/>.
    /// </summary>
    [TestClass]
    public class BoxViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Post"/>.
        /// </summary>
        private FakePostService postService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Box"/>.
        /// </summary>
        private FakeBoxService boxService;

        /// <summary>
        ///     Stock le service de navigation fictif.
        /// </summary>
        private FakeNavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de stockage local.
        /// </summary>
        private FakeStorageService storageService;

        /// <summary>
        ///     Stock le service fictif d'affichage de popups.
        /// </summary>
        private FakeDialogService dialogService;

        /// <summary>
        ///     Stock le view model de la page d'une boite (ici le view model à tester).
        /// </summary>
        private BoxViewModel boxViewModel;

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
            this.postService = new FakePostService();
            this.boxService = new FakeBoxService();
            this.navigationService = new FakeNavigationService();
            this.storageService = new FakeStorageService();
            this.dialogService = new FakeDialogService();

            this.boxViewModel = new BoxViewModel(this.postService, this.boxService,
                this.navigationService, this.storageService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.postService = null;
            this.boxService = null;
            this.navigationService = null;
            this.storageService = null;
            this.dialogService = null;

            this.boxViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Id</c> est la valeur de la propriété <c>Id</c> de la boite en paramètre.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_IdIsBoxId()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Id, this.boxViewModel.Id);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Title</c> est la valeur de la propriété <c>Title</c> de la boite en
        ///     paramètre.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_TitleIsBoxTitle()
        {
            // Arrange
            var box = new Box { Title = "Lorem" };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Title, this.boxViewModel.Title);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Description</c> est la valeur de la propriété <c>Description</c> de la
        ///     boite en paramètre.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_DescriptionIsBoxDescription()
        {
            // Arrange
            var box = new Box { Description = "Lorem ipsum" };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Description, this.boxViewModel.Description);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la valeur de la propriété
        ///     <c>Creator</c> est la valeur de la propriété <c>Creator</c> de la boite en
        ///     paramètre.
        /// </summary>
        [TestMethod]
        public void Initialize_BoxAsParameter_CreatorIsBoxCreator()
        {
            // Arrange
            var box = new Box { Creator = new User { Id = random.Next(50) } };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(box.Creator, this.boxViewModel.Creator);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, si l'utilisateur courant
        ///     est dans la liste des abonnés de la boite, la propriété <c>IsUserSubscribed</c>
        ///     est vraie.
        /// </summary>
        [TestMethod]
        public void Initialize_UserSubscribed_IsSubscribedTrue()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box();
            box.Subscribers.Add(user);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(this.boxViewModel.IsUserSubscribed);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, si l'utilisateur courant
        ///     n'est pas dans la liste des abonnés de la boite, la propriété
        ///     <c>IsUserSubscribed</c> est fausse.
        /// </summary>
        [TestMethod]
        public void Initialize_UserNotSubscribed_IsSubscribedFalse()
        {
            // Arrange
            var box = new Box();
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsFalse(this.boxViewModel.IsUserSubscribed);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, si l'utilisateur courant
        ///     est le créateur de la boite, la propriété <c>IsUserCreated</c> est vraie.
        /// </summary>
        [TestMethod]
        public void Initialize_UserIsCreator_IsUserCreatedTrue()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box { Creator = user };
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(this.boxViewModel.IsUserCreated);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, si l'utilisateur courant
        ///     n'est pas le créateur de la boite, la propriété <c>IsUserCreated</c> est
        ///     fausse.
        /// </summary>
        [TestMethod]
        public void Initialize_UserIsNotCreator_IsUserCreatedFalse()
        {
            // Arrange
            var box = new Box();
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsFalse(this.boxViewModel.IsUserCreated);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de type
        ///     <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToBox_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var box = new Box();
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, un message de type
        ///     <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToBox_ShellTitleMessageSent()
        {
            // Arrange
            var box = new Box();
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(this, m => wasShellTitleMessageSent = true);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lors de l'initialisation du view model, la liste des posts
        ///     de la boite n'est pas vide.
        /// </summary>
        [TestMethod]
        public async Task Initialize_NavigationToBox_PostsNotEmpty()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };
            await this.postService.CreateAsync(new Post
            {
                Id = random.Next(50, 100),
                Box = box
            });
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.boxViewModel.Initialize(box);

            // Assert
            Assert.AreEqual(1, this.boxViewModel.Posts.Count, "Aucun post n'a été récupéré.");
            Assert.IsNotNull(this.boxViewModel.Posts[0], "Le post n'est pas accessible.");
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page, un message de type
        ///     <see cref="IsBackButtonVisibleMessage"/> est envoyé.
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
            this.boxViewModel.Cleanup();

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        #endregion

        #region Show post command

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur souhaite voir le détail d'un post et qu'il
        ///     fait appel à la commande d'affichage d'un post, la page actuellement affichée
        ///     est bien celle du post.
        /// </summary>
        [TestMethod]
        public void PostCommand_GoToPostPage_CurrentPageIsPost()
        {
            // Arrange
            var post = new Post();
            this.navigationService.NavigateTo("Box");

            // Act
            this.boxViewModel.ShowPostCommand.Execute(post);

            // Assert
            Assert.AreEqual("Post", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur souhaite voir le détail d'un post et qu'il
        ///     fait appel à la commande d'affichage de ce post, le paramètre transmis à la
        ///     page actuellement affichée est bien le post en paramètre.
        /// </summary>
        [TestMethod]
        public void PostCommand_GoToPostPage_CurrentPageParameterIsPostToShow()
        {
            // Arrange
            var post = new Post { Id = random.Next(50) };
            this.navigationService.NavigateTo("Box");

            // Act
            this.boxViewModel.ShowPostCommand.Execute(post);

            // Assert
            Assert.IsInstanceOfType(this.navigationService.CurrentPageParameter, typeof(Post));
            Assert.AreEqual(post.Id, (this.navigationService.CurrentPageParameter as Post).Id);
        }

        #endregion

        #region Create post command

        /// <summary>
        ///     Vérifie que lorsque la commande de création d'un post est appelée et que le
        ///     contenu du post à créer est vide (ou succession d'espaces), cette dernière ne
        ///     peut s'exécuter.
        /// </summary>
        /// <param name="postContent">
        ///     Contenu du post à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void CreatePostCommand_PostContentIsNullOrWhiteSpace_CannotExecute(string postContent)
        {
            // Arrange
            this.boxViewModel.IsPosting = false;

            // Act
            bool canCreatePostExecute = this.boxViewModel.CreatePostCommand.CanExecute(postContent);

            // Assert
            Assert.IsFalse(canCreatePostExecute);
        }

        /// <summary>
        ///     Vérifie que lorsqu'un post est en cours de création, la commande de création
        ///     d'un post ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreatePostCommand_IsPosting_CannotExecute()
        {
            // Arrange
            this.boxViewModel.IsPosting = true;
            var postContent = new string('*', 10);

            // Act
            bool canCreatePostExecute = this.boxViewModel.CreatePostCommand.CanExecute(postContent);

            // Assert
            Assert.IsFalse(canCreatePostExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de création d'un post est appelée, que le
        ///     contenu du post à créer n'est pas vide (ou succession d'espaces) et qu'aucun
        ///     post n'est en cours de création, la commande de création peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreatePostCommand_IsNotPostingContentNotNullOrWhiteSpace_CanExecute()
        {
            // Arrange
            this.boxViewModel.IsPosting = false;
            var postContent = new string('*', 10);

            // Act
            bool canCreatePostExecute = this.boxViewModel.CreatePostCommand.CanExecute(postContent);

            // Assert
            Assert.IsTrue(canCreatePostExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de création d'un post est appelée avec des
        ///     données valides, le post est bien créé.
        /// </summary>
        [TestMethod]
        public void CreatePostCommand_ValidData_PostCreated()
        {
            // Arrange
            var user = new User();
            this.boxViewModel.Id = random.Next(50);
            var postContent = new string('*', 10);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.CreatePostCommand.Execute(postContent);

            // Assert
            Assert.AreEqual(1, this.postService.Posts.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de création d'un post est appelée avec des
        ///     données valides, le post créé est bien ajouté dans la liste <c>Posts</c> du
        ///     view model.
        /// </summary>
        [TestMethod]
        public void CreatePostCommand_ValidData_PostsNotEmpty()
        {
            // Arrange
            var user = new User();
            this.boxViewModel.Id = random.Next(50);
            var postContent = new string('*', 10);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.CreatePostCommand.Execute(postContent);

            // Assert
            Assert.AreEqual(1, this.boxViewModel.Posts.Count);
        }

        #endregion

        #region Subscribe command

        /// <summary>
        ///     Vérifie que lorsqu'un abonnement est en cours, la commande d'abonnement
        ///     ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void SubscribeCommand_IsSubscribing_CannotExecute()
        {
            // Arrange
            this.boxViewModel.IsSubscribing = true;

            // Act
            bool canSubscribeExecute = this.boxViewModel.SubscribeCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canSubscribeExecute);
        }

        /// <summary>
        ///     Vérifie que lorsqu'aucun abonnement n'est en cours, la commande
        ///     d'abonnement peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void SubscribeCommand_IsNotSubscribing_CanExecute()
        {
            // Arrange
            this.boxViewModel.IsSubscribing = false;

            // Act
            bool canSubscribeExecute = this.boxViewModel.SubscribeCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canSubscribeExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande d'abonnement est appelée, l'utilisateur
        ///     courant est bien rattaché à la boite.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task SubscribeCommand_UserNotSubscribed_UserAttachedToBox()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box { Id = random.Next(50, 100) };
            this.boxViewModel.Id = box.Id;
            await this.boxService.CreateAsync(box);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.SubscribeCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, this.boxService.Boxes[0].Subscribers.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande d'abonnement est appelée, la valeur de
        ///     la propriété <c>IsUserSubscribed</c> du view model est bien à true.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task SubscribeCommand_UserNotSubscribed_IsUserSubscribedTrue()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box { Id = random.Next(50, 100) };
            this.boxViewModel.Id = box.Id;
            this.boxViewModel.IsUserSubscribed = false;
            await this.boxService.CreateAsync(box);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.SubscribeCommand.Execute(null);

            // Assert
            Assert.IsTrue(this.boxViewModel.IsUserSubscribed);
        }

        #endregion

        #region Unsubscribe command

        /// <summary>
        ///     Vérifie que lorsqu'un désabonnement est en cours, la commande de désabonnement
        ///     ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void UnsubscribeCommand_IsUnsubscribing_CannotExecute()
        {
            // Arrange
            this.boxViewModel.IsUnsubscribing = true;

            // Act
            bool canUnsubscribeExecute = this.boxViewModel.UnsubscribeCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canUnsubscribeExecute);
        }

        /// <summary>
        ///     Vérifie que lorsqu'aucun désabonnement n'est en cours, la commande de
        ///     désabonnement peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void UnsubscribeCommand_IsNotUnsubscribing_CanExecute()
        {
            // Arrange
            this.boxViewModel.IsUnsubscribing = false;

            // Act
            bool canUnsubscribeExecute = this.boxViewModel.UnsubscribeCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canUnsubscribeExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de désabonnement est appelée, l'utilisateur
        ///     courant n'est plus rattaché à la boite.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task UnsubscribeCommand_UserSubscribed_UserDetachedFromBox()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box
            {
                Id = random.Next(50, 100),
                Subscribers = new List<User> { user }
            };
            this.boxViewModel.Id = box.Id;
            await this.boxService.CreateAsync(box);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.UnsubscribeCommand.Execute(null);

            // Assert
            Assert.AreEqual(0, this.boxService.Boxes[0].Subscribers.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de désabonnement est appelée, la valeur de
        ///     la propriété <c>IsUserSubscribed</c> du view model est à false.
        /// </summary>
        /// <returns>
        ///     Tache asynchrone qui permet d'attendre la fin des opérations.
        /// </returns>
        [TestMethod]
        public async Task UnsubscribeCommand_UserSubscribed_IsUserSubscribedFalse()
        {
            // Arrange
            var user = new User { Id = random.Next(50) };
            var box = new Box
            {
                Id = random.Next(50, 100),
                Subscribers = new List<User> { user }
            };
            this.boxViewModel.Id = box.Id;
            this.boxViewModel.IsUserSubscribed = true;
            await this.boxService.CreateAsync(box);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(user));

            // Act
            this.boxViewModel.UnsubscribeCommand.Execute(null);

            // Assert
            Assert.IsFalse(this.boxViewModel.IsUserSubscribed);
        }

        #endregion

        #region Show edit box command

        /// <summary>
        ///     Vérifie que lorsque la commande d'affichage de la page d'édition d'une boite est
        ///     appelée, la page actuellement affichée est la page d'édition d'une boite.
        /// </summary>
        [TestMethod]
        public void ShowEditBoxCommand_GoToEditBox_CurrentPageIsEditBox()
        {
            // Arrange
            this.navigationService.NavigateTo("Box");

            // Act
            this.boxViewModel.ShowEditBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual("EditBox", this.navigationService.CurrentPageKey);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande d'affichage de la page d'édition d'une boite est
        ///     appelée, le paramètre transmis à la page actuellement affichée est bien la boite
        ///     à modifier.
        /// </summary>
        [TestMethod]
        public void ShowEditBoxCommand_GoToEditBox_CurrentPageParameterIsBoxToEdit()
        {
            // Arrange
            this.boxViewModel.Id = random.Next(50);
            this.boxViewModel.Title = new string('*', 10);
            this.boxViewModel.Description = new string('*', 50);

            // Act
            this.boxViewModel.ShowEditBoxCommand.Execute(null);

            // Assert
            var pageParameter = this.navigationService.CurrentPageParameter as Box;
            Assert.AreEqual(this.boxViewModel.Id, pageParameter.Id);
            Assert.AreEqual(this.boxViewModel.Title, pageParameter.Title);
            Assert.AreEqual(this.boxViewModel.Description, pageParameter.Description);
        }

        #endregion

        #region Delete box command

        /// <summary>
        ///     Vérifie que la commande de suppression ne peut s'exécuter lorsque la suppression
        ///     est déjà en cours.
        /// </summary>
        [TestMethod]
        public void DeleteBoxCommand_IsDeleting_CannotExecute()
        {
            // Arrange
            this.boxViewModel.IsDeleting = true;

            // Act
            bool canDeleteBoxCommand = this.boxViewModel.DeleteBoxCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canDeleteBoxCommand);
        }

        /// <summary>
        ///     Vérifie que la commande de suppression peut s'exécuter lorsqu'aucune suppression
        ///     n'est en cours.
        /// </summary>
        [TestMethod]
        public void DeleteBoxCommand_IsNotDeleting_CanExecute()
        {
            // Arrange
            this.boxViewModel.IsDeleting = false;

            // Act
            bool canDeleteBoxCommand = this.boxViewModel.DeleteBoxCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canDeleteBoxCommand);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de suppression est appelée, la boite est bien
        ///     supprimée.
        /// </summary>
        [TestMethod]
        public async Task DeleteBoxCommand_UserIsCreator_BoxDeleted()
        {
            // Arrange
            var box = new Box { Id = random.Next(50) };
            this.boxViewModel.Id = box.Id;
            await this.boxService.CreateAsync(box);

            // Act
            this.boxViewModel.DeleteBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual(0, this.boxService.Boxes.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque la commande de suppression est appelée, la page actuellement
        ///     affichée est la page précédente.
        /// </summary>
        [TestMethod]
        public void DeleteBoxCommand_UserIsCreator_CurrentPageIsRootPage()
        {
            // Arrange
            this.navigationService.NavigateTo("Box");

            // Act
            this.boxViewModel.DeleteBoxCommand.Execute(null);

            // Assert
            Assert.AreEqual("RootPage", this.navigationService.CurrentPageKey);
        }

        #endregion
    }
}
