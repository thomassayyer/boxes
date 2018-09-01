using Boxes.Services.Box;
using Boxes.Models;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Boxes.Auxiliary.Exceptions;
using Boxes.Auxiliary.Messaging;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Boxes.Services.Localization;

namespace Boxes.ViewModels
{
    /// <summary>
    ///     View model de la page "Découvrir".
    /// </summary>
    public class DiscoverViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Stock le service d'accès aux données de l'entité <see cref="Box"/>.
        /// </summary>
        private readonly IBoxService boxService;

        /// <summary>
        ///     Stock le service de navigation.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        ///     Stock le service d'accès aux données de localization.
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
        ///     Stock la valeur de la propriété <c>IsSearching</c>.
        /// </summary>
        private bool isSearching;

        /// <summary>
        ///     Stock la valeur de la propriété <c>TopBoxes</c>.
        /// </summary>
        private ObservableCollection<Box> topBoxes;

        /// <summary>
        ///     Stock la valeur de la propriété <c>SearchResults</c>.
        /// </summary>
        private ObservableCollection<Box> searchResults;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur de paramétré qui initialize les propriétés du view model.
        /// </summary>
        /// <param name="boxService">
        ///     Instance du service d'accès aux données de l'entité <see cref="Box"/>.
        /// </param>
        /// <param name="navigationService">
        ///     Instance du service de navigation.
        /// </param>
        /// <param name="localizationService">
        ///     Instance du service de localization.
        /// </param>
        /// <param name="dialogService">
        ///     Instance du service d'affichage de popups.
        /// </param>
        public DiscoverViewModel(IBoxService boxService, INavigationService navigationService,
            ILocalizationService localizationService, IDialogService dialogService)
        {
            this.boxService = boxService;
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.dialogService = dialogService;

            this.ShowBoxCommand = new RelayCommand<Box>(this.ShowBox);
            this.SearchBoxCommand = new RelayCommand<string>(this.SearchBox);

            this.IsLoading = false;
            this.IsSearching = false;
            this.TopBoxes = new ObservableCollection<Box>();
            this.SearchResults = new ObservableCollection<Box>();

            if (this.IsInDesignMode)
                this.ReloadTopBoxes();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Est-ce que la page est en train de charger ?
        /// </summary>
        public bool IsLoading
        {
            get { return this.isLoading; }
            set { this.Set(() => this.IsLoading, ref this.isLoading, value); }
        }

        /// <summary>
        ///     Est-ce que l'utilisateur est en train de faire une recherche ?
        /// </summary>
        public bool IsSearching
        {
            get { return this.isSearching; }
            set { this.Set(() => this.IsSearching, ref this.isSearching, value); }
        }

        /// <summary>
        ///     Liste des meilleures boites.
        /// </summary>
        public ObservableCollection<Box> TopBoxes
        {
            get { return this.topBoxes; }
            set { this.Set(() => this.TopBoxes, ref this.topBoxes, value); }
        }

        /// <summary>
        ///     Résultat de la recherche effectuée par l'utilisateur.
        /// </summary>
        public ObservableCollection<Box> SearchResults
        {
            get { return this.searchResults; }
            set { this.Set(() => this.SearchResults, ref this.searchResults, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Commande d'affichage d'une boite.
        /// </summary>
        public RelayCommand<Box> ShowBoxCommand { get; private set; }

        /// <summary>
        ///     Commande de recherche d'une boite.
        /// </summary>
        public RelayCommand<string> SearchBoxCommand { get; private set; }

        #endregion

        #region Initialize / Cleanup

        /// <summary>
        ///     Exécute les initialisations pour chaque navigation qui s'effectue sur
        ///     la page "Découvrir".
        /// </summary>
        public void Initialize()
        {
            this.ReloadTopBoxes();

            // Demande le changement de titre du Shell.
            this.MessengerInstance.Send(new ShellTitleMessage(this.localizationService.GetString("Discover")));
        }

        #endregion

        #region Top boxes

        /// <summary>
        ///     Recharge le top des boites.
        /// </summary>
        private async void ReloadTopBoxes()
        {
            try
            {
                this.IsLoading = true;

                this.TopBoxes = new ObservableCollection<Box>(await this.boxService.GetTopAsync());
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

        #endregion

        #region Show box

        /// <summary>
        ///     Navigue vers la page de détails d'une boite.
        /// </summary>
        /// <param name="box">
        ///     Boite à afficher.
        /// </param>
        private void ShowBox(Box box)
        {
            this.navigationService.NavigateTo("Box", box);
        }

        #endregion

        #region Search

        /// <summary>
        ///     Recherche une boite.
        /// </summary>
        /// <param name="terms">
        ///     Termes de la recherche de l'utilisateur.
        /// </param>
        private async void SearchBox(string terms)
        {
            try
            {
                // On commence la recherche à partir de 2 caractères.
                if (terms.Length <= 2)
                {
                    this.IsSearching = false;
                    return;
                }

                this.IsLoading = true;
                this.IsSearching = true;

                List<Box> results = await this.boxService.GetSearchResultsAsync(terms);
                this.SearchResults = new ObservableCollection<Box>(results);
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

        #endregion
    }
}
