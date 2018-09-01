using Boxes.Services.Box;
using Boxes.Services.Comment;
using Boxes.Services.Localization;
using Boxes.Services.Post;
using Boxes.Services.Storage;
using Boxes.Services.User;
using Boxes.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     Cette classe recense les view models et services de l'application.
    /// </summary>
    /// <remarks>
    ///     Elle permet une communication simple entre les view models, les views et les services.
    ///     En effet, cette classe permet l'injection de certaines dépendances notamment les services.
    /// </remarks>
    class ViewModelLocator
    {
        /// <summary>
        ///     Constructeur statique qui charge tous les view models et service dans
        ///     le conteneur de dépendance.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Mode design pour Visual Studio et Blend.
                SimpleIoc.Default.Register<IStorageService, DesignStorageService>();
                SimpleIoc.Default.Register<IUserService, DesignUserService>();
                SimpleIoc.Default.Register<IBoxService, DesignBoxService>();
                SimpleIoc.Default.Register<IPostService, DesignPostService>();
                SimpleIoc.Default.Register<ICommentService, DesignCommentService>();
            }
            else
            {
                // Mode Debug / Run.
                SimpleIoc.Default.Register<IStorageService, StorageService>();
                SimpleIoc.Default.Register<IUserService, UserService>();
                SimpleIoc.Default.Register<IBoxService, BoxService>();
                SimpleIoc.Default.Register<IPostService, PostService>();
                SimpleIoc.Default.Register<ICommentService, CommentService>();
            }

            // Services.
            SimpleIoc.Default.Register(() => CreateNavigationService());
            SimpleIoc.Default.Register<ILocalizationService, LocalizationService>();
            SimpleIoc.Default.Register<IDialogService, Services.Dialog.DialogService>();

            // View models.
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<RegisterViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<PostViewModel>();
            SimpleIoc.Default.Register<DiscoverViewModel>();
            SimpleIoc.Default.Register<BoxViewModel>();
            SimpleIoc.Default.Register<MyBoxesViewModel>();
            SimpleIoc.Default.Register<CreateBoxViewModel>();
            SimpleIoc.Default.Register<EditBoxViewModel>();
        }

        /// <summary>
        ///     Instance de <see cref="LoginViewModel"/> du conteneur de dépendance.
        /// </summary>
        public LoginViewModel LoginVM
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="RegisterViewModel"/> du conteneur de dépendance.
        /// </summary>
        public RegisterViewModel RegisterVM
        {
            get { return ServiceLocator.Current.GetInstance<RegisterViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="ShellViewModel"/> du conteneur de dépendance.
        /// </summary>
        public ShellViewModel ShellVM
        {
            get { return ServiceLocator.Current.GetInstance<ShellViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="HomeViewModel"/> du conteneur de dépendance.
        /// </summary>
        public HomeViewModel HomeVM
        {
            get { return ServiceLocator.Current.GetInstance<HomeViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="PostViewModel"/> du conteneur de dépendance.
        /// </summary>
        public PostViewModel PostVM
        {
            get { return ServiceLocator.Current.GetInstance<PostViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="DiscoverViewModel"/> du conteneur de dépendance.
        /// </summary>
        public DiscoverViewModel DiscoverVM
        {
            get { return ServiceLocator.Current.GetInstance<DiscoverViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="BoxViewModel"/> du conteneur de dépendance.
        /// </summary>
        public BoxViewModel BoxVM
        {
            get { return ServiceLocator.Current.GetInstance<BoxViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="MyBoxesViewModel"/> du conteneur de dépendance.
        /// </summary>
        public MyBoxesViewModel MyBoxesVM
        {
            get { return ServiceLocator.Current.GetInstance<MyBoxesViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="CreateBoxViewModel"/> du conteneur de dépendance.
        /// </summary>
        public CreateBoxViewModel CreateBoxVM
        {
            get { return ServiceLocator.Current.GetInstance<CreateBoxViewModel>(); }
        }

        /// <summary>
        ///     Instance de <see cref="EditBoxViewModel"/> du conteneur de dépendance.
        /// </summary>
        public EditBoxViewModel EditBoxVM
        {
            get { return ServiceLocator.Current.GetInstance<EditBoxViewModel>(); }
        }

        /// <summary>
        ///     Crée un service de navigation.
        /// </summary>
        /// <remarks>
        ///     C'est dans cette méthode que son configurées les vues du service de
        ///     navigation.
        /// </remarks>
        /// <returns>
        ///     Un service de navigation configuré.
        /// </returns>
        private static INavigationService CreateNavigationService()
        {
            var navigationService = new Services.Navigation.NavigationService();

            navigationService.Configure("Login", typeof(LoginPage), false);
            navigationService.Configure("Register", typeof(RegisterPage), false);
            navigationService.Configure("Shell", typeof(Shell), false);
            navigationService.Configure("Home", typeof(HomePage), true);
            navigationService.Configure("Post", typeof(PostPage), true);
            navigationService.Configure("Discover", typeof(DiscoverPage), true);
            navigationService.Configure("Box", typeof(BoxPage), true);
            navigationService.Configure("MyBoxes", typeof(MyBoxesPage), true);
            navigationService.Configure("CreateBox", typeof(CreateBoxPage), true);
            navigationService.Configure("EditBox", typeof(EditBoxPage), true);

            return navigationService;
        }
    }
}
