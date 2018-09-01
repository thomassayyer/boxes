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
    ///     Effectue les tests unitaires du view model <see cref="PostViewModel"/>.
    /// </summary>
    [TestClass]
    public class PostViewModelTests
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Post"/>.
        /// </summary>
        private FakePostService postService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de l'entité <see cref="Comment"/>.
        /// </summary>
        private FakeCommentService commentService;

        /// <summary>
        ///     Stock le service d'accès aux données fictives de stockage local.
        /// </summary>
        private FakeStorageService storageService;

        /// <summary>
        ///     Stock le service fictif d'affichage de popups.
        /// </summary>
        private FakeDialogService dialogService;

        /// <summary>
        ///     Stock le view model de la page d'un post (ici le view model à tester).
        /// </summary>
        private PostViewModel postViewModel;

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
            this.postService = new FakePostService();
            this.commentService = new FakeCommentService();
            this.storageService = new FakeStorageService();
            this.dialogService = new FakeDialogService();

            this.postViewModel = new PostViewModel(this.postService, this.commentService,
                this.storageService, this.dialogService);
        }

        /// <summary>
        ///     Effectue les nettoyages de variables devant être faits après l'exécution
        ///     de chaque test.
        /// </summary>
        [TestCleanup]
        public void TestsCleanup()
        {
            this.postService = null;
            this.commentService = null;
            this.storageService = null;
            this.dialogService = null;

            this.postViewModel = null;
        }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la valeur
        ///     de la propriété <c>Id</c> du view model est bien celle du post transmis
        ///     lors de l'initialisation.
        /// </summary>
        [TestMethod]
        public void Initialize_PostAsParameter_IdIsPostId()
        {
            var post = new Post { Id = random.Next(50) };

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(post.Id, this.postViewModel.Id);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la valeur
        ///     de la propriété <c>Content</c> du view model est bien celle du post
        ///     transmis lors de l'initialisation.
        /// </summary>
        [TestMethod]
        public void Initialize_PostAsParameter_ContentIsPostContent()
        {
            // Arrange
            var post = new Post { Content = "Lorem ipsum" };

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(post.Content, this.postViewModel.Content);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la valeur
        ///     de la propriété <c>Author</c> du view model est bien celle du post
        ///     transmis lors de l'initialisation.
        /// </summary>
        [TestMethod]
        public void Initialize_PostAsParameter_AuthorIsPostAuthor()
        {
            // Arrange
            var post = new Post
            {
                Author = new User { Id = random.Next(50) }
            };

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(post.Author.Id, this.postViewModel.Author.Id);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la valeur
        ///     de la propriété <c>Box</c> du view model est bien celle du post transmis
        ///     lors de l'initialisation.
        /// </summary>
        [TestMethod]
        public void Initialize_PostAsParameter_BoxIsPostBox()
        {
            // Arrange
            var post = new Post
            {
                Box = new Box { Id = random.Next(50) }
            };

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(post.Box.Id, postViewModel.Box.Id);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la valeur
        ///     de la propriété <c>CreatedAt</c> du view model est bien celle du post
        ///     transmis lors de l'initialisation.
        /// </summary>
        [TestMethod]
        public void Initialize_PostAsParameter_CreatedAtIsPostCreatedAt()
        {
            // Arrange
            var post = new Post { CreatedAt = DateTime.Now };

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(post.CreatedAt, postViewModel.CreatedAt);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, la liste
        ///     des commentaires du post n'est pas vide.
        /// </summary>
        /// <returns>
        ///     Opération asynchrone qui permet d'attendre la fin de l'exécution des
        ///     opérations.
        /// </returns>
        [TestMethod]
        public async Task Initialize_NavigationToPost_CommentsNotEmpty()
        {
            // Arrange
            var post = new Post { Id = random.Next(50) };
            await this.commentService.CreateAsync(new Comment
            {
                Id = random.Next(50, 100),
                Post = post
            });

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.AreEqual(1, this.commentService.Comments.Count);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, un message
        ///     de type <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToPost_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var post = new Post();
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur entre sur la page d'un post, un message
        ///     de type <see cref="ShellTitleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Initialize_NavigationToPost_ShellTitleMessageSent()
        {
            // Arrange
            var post = new Post();
            var wasShellTitleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<ShellTitleMessage>(
                this, m => wasShellTitleMessageSent = true);

            // Act
            this.postViewModel.Initialize(post);

            // Assert
            Assert.IsTrue(wasShellTitleMessageSent);
        }

        /// <summary>
        ///     Vérifie que lorsque l'utilisateur quitte la page d'un post, un message
        ///     de type <see cref="IsBackButtonVisibleMessage"/> est envoyé.
        /// </summary>
        [TestMethod]
        public void Cleanup_NavigationFromPost_IsBackButtonVisibleMessageSent()
        {
            // Arrange
            var wasIsBackButtonVisibleMessageSent = false;
            Messenger.Reset();
            Messenger.Default.Register<IsBackButtonVisibleMessage>(
                this, m => wasIsBackButtonVisibleMessageSent = true);

            // Act
            this.postViewModel.Cleanup();

            // Assert
            Assert.IsTrue(wasIsBackButtonVisibleMessageSent);
        }

        #endregion

        #region Create comment command

        /// <summary>
        ///     Vérifie que lorsque le contenu du commentaire à créer est vide, la
        ///     commande de création ne peut s'exécuter.
        /// </summary>
        /// <param name="content">
        ///     Contenu du commentaire à tester.
        /// </param>
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void CreateCommentCommand_ContentIsNullOrWhiteSpace_CannotExecute(string content)
        {
            // Arrange
            this.postViewModel.IsCommenting = false;
            var comment = content;

            // Act
            var canCreateCommentExecute = this.postViewModel.CreateCommentCommand.CanExecute(comment);

            // Assert
            Assert.IsFalse(canCreateCommentExecute);
        }

        /// <summary>
        ///     Vérifie que lorsqu'un commentaire est en cours d'envoi, la commande
        ///     de création ne peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreateCommentCommand_IsCommenting_CannotExecute()
        {
            // Arrange
            this.postViewModel.IsCommenting = true;
            var comment = new string('*', 10);

            // Act
            var canCreateCommentExecute = this.postViewModel.CreateCommentCommand.CanExecute(comment);

            // Assert
            Assert.IsFalse(canCreateCommentExecute);
        }

        /// <summary>
        ///     Vérifie que lorsque le contenu du commentaire à créer n'est pas vide,
        ///     la commande de création peut s'exécuter.
        /// </summary>
        [TestMethod]
        public void CreateCommentCommand_IsNotCommentingContentNotNullOrEmpty_CanExecute()
        {
            // Arrange
            this.postViewModel.IsCommenting = false;
            var comment = new string('*', 10);

            // Act
            var canCreateCommentExecute = this.postViewModel.CreateCommentCommand.CanExecute(comment);

            // Assert
            Assert.IsTrue(canCreateCommentExecute);
        }

        /// <summary>
        ///     Vérifie que lors de l'appel à la commande de création d'un commentaire,
        ///     un commentaire est bien créé.
        /// </summary>
        [TestMethod]
        public void CreateCommentCommand_ContentNotNullOrWhiteSpace_CommentCreated()
        {
            // Arrange
            this.postViewModel.Id = random.Next(50);
            var content = new string('*', 10);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.postViewModel.CreateCommentCommand.Execute(content);

            // Assert
            Assert.AreEqual(1, this.commentService.Comments.Count);
        }

        /// <summary>
        ///     Vérifie qu'après la création d'un commentaire, la liste des commentaires
        ///     du view model n'est pas vide.
        /// </summary>
        [TestMethod]
        public void CreateCommentCommand_NotEmptyContent_CommentsNotEmpty()
        {
            // Arrange
            this.postViewModel.Id = 1;
            var content = new string('*', 10);
            this.storageService.SaveSetting("CurrentUser", JsonConvert.SerializeObject(new User()));

            // Act
            this.postViewModel.CreateCommentCommand.Execute(content);

            // Assert
            Assert.AreEqual(1, this.postViewModel.Comments.Count);
        }

        #endregion
    }
}
