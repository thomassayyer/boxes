using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Box;
using Boxes.Services.Post;
using Boxes.Services.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page d'une boite.
    /// </summary>
    public class BoxViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Post"/>.
        /// </summary>
        private readonly IPostService postService;

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Box"/>.
        /// </summary>
        private readonly IBoxService boxService;
        
        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;

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
        ///     Stock la valeur de la propriété <c>IsPosting</c>.
        /// </summary>
        private bool isPosting;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsSubscribing</c>.
        /// </summary>
        private bool isSubscribing;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsUnsubscribing</c>.
        /// </summary>
        private bool isUnsubscribing;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsDeleting</c>.
        /// </summary>
        private bool isDeleting;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Title</c>.
        /// </summary>
        private string title;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Description</c>.
        /// </summary>
        private string description;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Creator</c>.
        /// </summary>
        private User creator;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Posts</c>.
        /// </summary>
        private ObservableCollection<Post> posts;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsUserSubscribed</c>.
        /// </summary>
        private bool isUserSubscribed;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsUserCreated</c>.
        /// </summary>
        private bool isUserCreated;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur qui initialise les propriétés du view model.
        /// </summary>
        /// <param name="postService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Post"/>.
        /// </param>
        /// <param name="boxService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Box"/>.
        /// </param>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="storageService">
        ///     Instance du service d'accès aux données de stockage local.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service d'affichage de popups.
        /// </param>
        public BoxViewModel(IPostService postService, IBoxService boxService, INavigationService navigationService,
            IStorageService storageService, IDialogService dialogService)
        {
            this.postService = postService;
            this.boxService = boxService;
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.dialogService = dialogService;

            this.ShowPostCommand = new RelayCommand<Post>(this.ShowPost);
            this.CreatePostCommand = new RelayCommand<string>(this.CreatePost, this.CanCreatePostExecute);
            this.SubscribeCommand = new RelayCommand(this.Subscribe, this.CanSubscribeExecute);
            this.UnsubscribeCommand = new RelayCommand(this.Unsubscribe, this.CanUnsubscribeExecute);
            this.ShowEditBoxCommand = new RelayCommand(this.ShowEditBox);
            this.DeleteBoxCommand = new RelayCommand(this.DeleteBox, this.CanDeleteBoxExecute);

            this.IsLoading = false;
            this.IsPosting = false;
            this.IsSubscribing = false;
            this.IsUnsubscribing = false;
            this.Posts = new ObservableCollection<Post>();

            // Si nous sommes en mode design ==> chargement d'une boite fictive.
            if (this.IsInDesignMode)
            {
                this.Initialize(new Box
                {
                    Title = "Lorem",
                    Description = "Donec laoreet accumsan eros ut scelerisque. Integer sollicitudin justo nulla, a pulvinar nulla bibendum sit amet. Ut orci ex, viverra sit amet ornare varius, rutrum ac eros. Quisque consequat porttitor nulla, non convallis ex tristique sit amet. Morbi sed varius massa.",
                    Id = 1,
                    Subscribers = new List<User>
                    {
                        new User { FirstName = "David", LastName = "Pierce" },
                        new User { FirstName = "Thomas", LastName = "Edison" }
                    },
                    Creator = new User { FirstName = "John", LastName = "Doe", Id = 1 }
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
        ///     Est-ce que la création d'un post est en cours ?
        /// </summary>
        public bool IsPosting
        {
            get { return this.isPosting; }
            set
            {
                this.Set(() => this.IsPosting, ref this.isPosting, value);
                this.CreatePostCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Est-ce qu'un abonnement est en cours ?
        /// </summary>
        public bool IsSubscribing
        {
            get { return this.isSubscribing; }
            set
            {
                if (this.isSubscribing != value)
                {
                    this.isSubscribing = value;
                    this.SubscribeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Est-ce qu'un désabonnement est en cours ?
        /// </summary>
        public bool IsUnsubscribing
        {
            get { return this.isUnsubscribing; }
            set
            {
                if (this.isUnsubscribing != value)
                {
                    this.isUnsubscribing = value;
                    this.UnsubscribeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Est-ce que la boite est en cours de suppression ?
        /// </summary>
        public bool IsDeleting
        {
            get { return this.isDeleting; }
            set
            {
                if (this.isDeleting != value)
                {
                    this.isDeleting = value;
                    this.DeleteBoxCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Id de la boite.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Titre de la boite.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.Set(() => this.Title, ref this.title, value); }
        }

        /// <summary>
        ///     Description de la boite.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.Set(() => this.Description, ref this.description, value); }
        }

        /// <summary>
        ///     Créateur de la boite.
        /// </summary>
        public User Creator
        {
            get { return this.creator; }
            set { this.Set(() => this.Creator, ref this.creator, value); }
        }

        /// <summary>
        ///     Posts de la boite.
        /// </summary>
        public ObservableCollection<Post> Posts
        {
            get { return this.posts; }
            set { this.Set(() => this.Posts, ref this.posts, value); }
        }

        /// <summary>
        ///     Est-ce que l'utilisateur est abonné à cette boite ?
        /// </summary>
        public bool IsUserSubscribed
        {
            get { return this.isUserSubscribed; }
            set { this.Set(() => this.IsUserSubscribed, ref this.isUserSubscribed, value); }
        }

        /// <summary>
        ///     Est-ce que l'utilisateur a créée cette boite ?
        /// </summary>
        public bool IsUserCreated
        {
            get { return this.isUserCreated; }
            set { this.Set(() => this.IsUserCreated, ref this.isUserCreated, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande d'affichage d'un post.
        /// </summary>
        public RelayCommand<Post> ShowPostCommand { get; private set; }

        /// <summary>
        ///     Commande de création d'un post.
        /// </summary>
        public RelayCommand<string> CreatePostCommand { get; private set; }

        /// <summary>
        ///     Commande d'abonnement à la boite.
        /// </summary>
        public RelayCommand SubscribeCommand { get; private set; }

        /// <summary>
        ///     Commande de désabonnement à la boite.
        /// </summary>
        public RelayCommand UnsubscribeCommand { get; private set; }

        /// <summary>
        ///     Commande d'affichage de la fenêtre d'édition d'une boite.
        /// </summary>
        public RelayCommand ShowEditBoxCommand { get; private set; }

        /// <summary>
        ///     Commande de suppression d'une boite.
        /// </summary>
        public RelayCommand DeleteBoxCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Exécute les initialisations s'opérant pour chaque navigation sur la
        ///     page de la boite.
        /// </summary>
        /// <param name="box">
        ///     Boite à afficher.
        /// </param>
        public void Initialize(Box box)
        {
            this.Id = box.Id;
            this.Title = box.Title;
            this.Description = box.Description;
            this.Creator = box.Creator;

            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            // Vérifie si l'utilisateur courrant est abonné à la boite.
            this.IsUserSubscribed = box.Subscribers.Contains(user);

            this.IsUserCreated = user.Equals(this.Creator);

            // Demande l'affichage du bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage());

            // Demande le changement de titre du Shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.Title));

            this.ReloadPosts();
        }

        /// <summary>
        ///     Exécute les opération à effectuer à chaque fois que l'utilisateur
        ///     quitte la page de la boite.
        /// </summary>
        public override void Cleanup()
        {
            // Demande à cacher le bouton de retour arrière.
            this.MessengerInstance.Send(new IsBackButtonVisibleMessage(false));

            base.Cleanup();
        }

        #endregion

        #region Posts

        /// <summary>
        ///     Recharge les posts de la boite.
        /// </summary>
        private async void ReloadPosts()
        {
            try
            {
                this.IsLoading = true;

                this.Posts = new ObservableCollection<Post>(await this.postService.GetByBoxAsync(new Box { Id = this.Id }));
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
        ///     Navigue vers la page de détail d'un post.
        /// </summary>
        /// <param name="post">
        ///     Post à afficher.
        /// </param>
        private void ShowPost(Post post)
        {
            this.navigationService.NavigateTo("Post", post);
        }

        /// <summary>
        ///     Détermine si la commande de création d'un post peut s'exécuter.
        /// </summary>
        /// <param name="postContent">
        ///     Contenu du post à créer.
        /// </param>
        /// <returns>
        ///     Est-ce que la commande de création de post peut s'exécuter ?
        /// </returns>
        private bool CanCreatePostExecute(string postContent)
        {
            return !string.IsNullOrWhiteSpace(postContent) &&
                   !this.IsPosting;
        }

        /// <summary>
        ///     Crée un nouveau post.
        /// </summary>
        private async void CreatePost(string postContent)
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            var post = new Post
            {
                Content = postContent,
                Author = user,
                Box = new Box { Id = this.Id }
            };

            try
            {
                this.IsPosting = true;

                Post created = await this.postService.CreateAsync(post);
                this.Posts.Insert(0, created);
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsPosting = false;
            }
        }

        #endregion

        #region Subscription

        /// <summary>
        ///     Détermine si la commande d'abonnement de l'utilisateur peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande d'abonnement peut s'exécuter ?
        /// </returns>
        private bool CanSubscribeExecute()
        {
            return !this.IsSubscribing;
        }

        /// <summary>
        ///     Ajoute l'utilisateur à la liste des abonnés de la boite.
        /// </summary>
        private async void Subscribe()
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            try
            {
                this.IsSubscribing = true;

                await this.boxService.AttachUserAsync(new Box { Id = this.Id }, user);
                this.IsUserSubscribed = true;
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsSubscribing = false;
            }
        }

        /// <summary>
        ///     Détermine si la commande de désabonnement peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande de désabonnement peut s'exécuter ?
        /// </returns>
        private bool CanUnsubscribeExecute()
        {
            return !this.IsUnsubscribing;
        }

        /// <summary>
        ///     Supprime l'utilisateur de la liste des abonnés de la boite.
        /// </summary>
        private async void Unsubscribe()
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            try
            {
                this.IsUnsubscribing = true;

                await this.boxService.DetachUserAsync(new Box { Id = this.Id }, user);
                this.IsUserSubscribed = false;
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsUnsubscribing = false;
            }
        }

        #endregion

        #region Edit

        /// <summary>
        ///     Navigue vers la page d'édition d'une boite.
        /// </summary>
        private void ShowEditBox()
        {
            var box = new Box
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description
            };

            this.navigationService.NavigateTo("EditBox", box);
        }

        #endregion

        #region Delete

        /// <summary>
        ///     Détermine si la commande de suppression peut s'exécuter.
        /// </summary>
        /// <returns>
        ///     Est-ce que la commande de suppression peut s'exécuter.
        /// </returns>
        private bool CanDeleteBoxExecute()
        {
            return !this.IsDeleting;
        }

        /// <summary>
        ///     Supprime la boite.
        /// </summary>
        private async void DeleteBox()
        {
            try
            {
                this.IsDeleting = true;

                await this.boxService.DeleteAsync(new Box { Id = this.Id });

                this.navigationService.GoBack();
            }
            catch (WebServiceException e)
            {
                await this.dialogService.ShowError(e, "Oops !", "Ok", null);
            }
            finally
            {
                this.IsDeleting = false;
            }
        }

        #endregion
    }
}
