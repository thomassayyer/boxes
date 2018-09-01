using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using Boxes.Models;
using Boxes.Services.Localization;
using Boxes.Services.Post;
using Boxes.Services.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page d'accueil.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de stockage local.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Post"/>.
        /// </summary>
        private readonly IPostService postService;

        /// <summary>
        ///     Stock le service de localization.
        /// </summary>
        private readonly ILocalizationService localizationService;

        /// <summary>
        ///     Stock le service d'affichage de popups.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        ///     Stock la valeur de la propriété <c>IsLoading</c>.
        /// </summary>
        private bool isLoading;

        /// <summary>
        ///     Stock la valeur de la propriété <c>Posts</c>.
        /// </summary>
        private ObservableCollection<Post> posts;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur paramétré qui initialize les propriétés du view model.
        /// </summary>
        /// <param name="storageService">
        ///     Instance du service d'accès aux données de stockage local.
        /// </param>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="postService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Post" />.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service d'accès aux resources de localization.
        /// </param>
        /// <param name="dialogService">
        ///     Insance du service d'affichage de popups.
        /// </param>
        public HomeViewModel(IStorageService storageService, INavigationService navigationService,
            IPostService postService, ILocalizationService localizationService, IDialogService dialogService)
        {
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.postService = postService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.IsLoading = false;
            this.ShowPostCommand = new RelayCommand<Post>(this.ShowPost);

            if (this.IsInDesignMode)
                this.ReloadPosts();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Posts de la boite séléctionnée.
        /// </summary>
        public ObservableCollection<Post> Posts
        {
            get { return this.posts; }
            set { this.Set(() => this.Posts, ref this.posts, value); }
        }

        /// <summary>
        ///     Est-ce qu'un chargement s'opère ?
        /// </summary>
        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande d'affichage d'un post.
        /// </summary>
        public RelayCommand<Post> ShowPostCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Effectue les initialisation pour chaque navigation qui
        ///     s'effectue sur la page d'accueil.
        /// </summary>
        public void Initialize()
        {
            this.ReloadPosts();

            // Demande le changemen de titre du Shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.localizationService.GetString("Home")));
        }

        #endregion

        #region Posts

        /// <summary>
        ///     Recharge les posts de la boite séléctionnée.
        /// </summary>
        private async void ReloadPosts()
        {
            User user = JsonConvert.DeserializeObject<User>(this.storageService.ReadSetting<string>("CurrentUser"));

            try
            {
                this.IsLoading = true;

                this.Posts = new ObservableCollection<Post>(await this.postService.GetByUserAsync(user));
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
        /// Navigue vers la page de détail d'un post.
        /// </summary>
        /// <param name="post">Post à afficher.</param>
        private void ShowPost(Post post)
        {
            this.navigationService.NavigateTo("Post", post);
        }

        #endregion
    }
}
