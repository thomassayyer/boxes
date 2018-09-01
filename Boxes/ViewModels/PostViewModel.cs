using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Comment;
using Boxes.Services.Post;
using Boxes.Services.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page d'un post.
    /// </summary>
    public class PostViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Models.Post"/>
        /// </summary>
        private readonly IPostService postService;

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Comment"/>
        /// </summary>
        private readonly ICommentService commentService;

        /// <summary>
        ///     Stock le service d'accès aux données de stockage local.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        ///     Stock le service d'affichage de popups.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsLoading</c>.
        /// </summary>
        private bool isLoading;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsCommenting</c>.
        /// </summary>
        private bool isCommenting;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Content</c>.
        /// </summary>
        private string content;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Author</c>.
        /// </summary>
        private User author;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Box</c>.
        /// </summary>
        private Box box;

        /// <summary>
        ///     Stock la valeur de la propriété <c>CreatedAt</c>.
        /// </summary>
        private DateTime createdAt;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Comments</c>.
        /// </summary>
        private ObservableCollection<Comment> comments;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialize les propriétés du view model.
        /// </summary>
        /// <param name="postService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Post"/>.
        /// </param>
        /// <param name="commentService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Comment"/>.
        /// </param>
        /// <param name="storageService">
        ///     Instance du service d'accès aux données de stockage local.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service d'affichage de popups.
        /// </param>
        public PostViewModel(IPostService postService, ICommentService commentService,
            IStorageService storageService, IDialogService dialogService)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.storageService = storageService;
            this.dialogService = dialogService;
            
            this.CreateCommentCommand = new RelayCommand<string>(this.CreateComment, this.CanCreateCommentExecute);

            this.IsLoading = false;
            this.IsCommenting = false;
            this.Comments = new ObservableCollection<Comment>();
            
            // Si nous sommes en mode design ==> chargement d'un post fictif.
            if (this.IsInDesignMode)
            {
                this.Initialize(new Post
                {
                    Id = 1,
                    Author = new User { FirstName = "John", LastName = "Doe" },
                    Content = "Lorem ipsum dolor sit amet",
                    CreatedAt = new DateTime()
                });
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Est-ce qu'un chargement s'opère ?
        /// </summary>
        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        /// <summary>
        ///     Est-ce qu'un commentaire est en cours de création ?
        /// </summary>
        public bool IsCommenting
        {
            get { return this.isCommenting; }
            set
            {
                this.Set(() => this.IsCommenting, ref this.isCommenting, value);
                this.CreateCommentCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Id du post.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Contenu du post.
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.Set(() => this.Content, ref this.content, value); }
        }

        /// <summary>
        ///     Auteur du post.
        /// </summary>
        public User Author
        {
            get { return this.author; }
            set { this.Set(() => this.Author, ref this.author, value); }
        }

        /// <summary>
        ///     Boite associée au post.
        /// </summary>
        public Box Box
        {
            get { return this.box; }
            set { this.Set(() => this.Box, ref this.box, value); }
        }

        /// <summary>
        ///     Date de création du post.
        /// </summary>
        public DateTime CreatedAt
        {
            get { return this.createdAt; }
            set { this.Set(() => this.CreatedAt, ref this.createdAt, value); }
        }

        /// <summary>
        ///     Commentaires associés au post.
        /// </summary>
        public ObservableCollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.Set(() => this.Comments, ref this.comments, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande de création d'un commentaire.
        /// </summary>
        public RelayCommand<string> CreateCommentCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation pour chaque navigation qui se fait vers
        ///     la page d'un post.
        /// </summary>
        /// <param name="post">
        ///     Post à afficher.
        /// </param>
        public void Initialize(Post post)
        {
            this.Id = post.Id;
            this.Content = post.Content;
            this.Author = post.Author;
            this.Box = post.Box;
            this.CreatedAt = post.CreatedAt;

            this.ReloadComments();

            // Demande l'affichage du bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage());

            // Demande le changement du titre du Shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.Author?.ToString()));
        }

        /// <summary>
        ///     Effectue les opération qui s'effectue à chaque fois que l'utilisateur
        ///     quitte la page d'un post.
        /// </summary>
        public override void Cleanup()
        {
            // Demande à cacher le bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage(false));

            base.Cleanup();
        }

        #endregion

        #region Comments

        /// <summary>
        ///     Recharge les commentaires.
        /// </summary>
        private async void ReloadComments()
        {
            try
            {
                this.IsLoading = true;

                this.Comments = new ObservableCollection<Comment>(await this.commentService.GetByPostAsync(new Post { Id = this.Id }));
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        ///     Détermine si la commande de création d'un commentaire peut s'exécuter.
        /// </summary>
        /// <param name="commentContent">
        ///     Contenu du champ commentaire.
        /// </param>
        /// <returns>
        ///     Est-ce que la commande de création d'un commentaire peut s'exécuter ?
        /// </returns>
        private bool CanCreateCommentExecute(string commentContent)
        {
            return !string.IsNullOrWhiteSpace(commentContent) &&
                   !this.IsCommenting;
        }

        /// <summary>
        ///     Enregistre un nouveau commentaire.
        /// </summary>
        private async void CreateComment(string commentContent)
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            var comment = new Comment
            {
                Content = commentContent,
                Author = user,
                Post = new Post { Id = this.Id }
            };

            try
            {
                this.IsCommenting = true;

                Comment created = await this.commentService.CreateAsync(comment);
                this.Comments.Insert(0, created);
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsCommenting = false;
            }
        }

        #endregion
    }
}
