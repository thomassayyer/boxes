using Boxes.Views;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Boxes.Services.Navigation
{
    /// <summary>
    ///     Service de navigation pour applications universelles utilisant un shell.
    /// </summary>
    class NavigationService : INavigationService
    {
        #region Fields

        /// <summary>
        ///     Contient les types des pages sur lesquelles naviguer avec leur clé associé.
        /// </summary>
        private Dictionary<string, Type> pagesByKey;

        /// <summary>
        ///     Pages étants destinées à être affichées dans le shell.
        /// </summary>
        private List<string> shellPages;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise les collections.
        /// </summary>
        public NavigationService()
        {
            this.pagesByKey = new Dictionary<string, Type>();
            this.shellPages = new List<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Frame du contenu du shell.
        /// </summary>
        private Frame ShellFrame
        {
            get
            {
                var rootFrame = (Frame)Window.Current.Content;
                return (rootFrame.Content as Shell)?.RootFrame;
            }
        }

        /// <summary>
        ///     Frame principale.
        /// </summary>
        private Frame RootFrame
        {
            get
            {
                return (Frame)Window.Current.Content;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Clé de la page actuellement affichée.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                Type currentType = this.RootFrame.Content.GetType();
                return this.pagesByKey.FirstOrDefault(i => i.Value == currentType).Key;
            }
        }

        #endregion

        #region Navigation

        /// <inheritdoc />
        /// <summary>
        ///     Si possible, affiche la page précédente (de la pile de navigation).
        /// </summary>
        public void GoBack()
        {
            // Si le shell est actuellement affiché et qu'il peut retourner en arrière.
            if (this.ShellFrame?.CanGoBack ?? false)
                this.ShellFrame.GoBack();
            else if (this.RootFrame.CanGoBack)
                this.RootFrame.GoBack();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Affiche la page correspondante à la clé donnée.
        /// </summary>
        /// <remarks>
        ///     Ne pas oublier d'appeler <see cref="Configure(string, Type, bool)"/>.
        /// </remarks>
        /// <param name="pageKey">
        ///     Clé de la page à afficher.
        /// </param>
        public void NavigateTo(string pageKey)
        {
            this.NavigateTo(pageKey, null);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Affiche la page correspondante à la clé donnée en lui passant un paramètre.
        /// </summary>
        /// <remarks>
        ///     Ne pas oublier d'appeler <see cref="Configure(string, Type, bool)"/>.
        /// </remarks>
        /// <param name="pageKey">
        ///     Clé de la page à afficher.
        /// </param>
        /// <param name="parameter">
        ///     Paramètres à transmettre à la nouvelle page.
        /// </param>
        public void NavigateTo(string pageKey, object parameter)
        {
            if (this.shellPages.Contains(pageKey))
                this.ShellFrame.Navigate(this.pagesByKey[pageKey], parameter);
            else
                this.RootFrame.Navigate(this.pagesByKey[pageKey], parameter);
        }

        #endregion

        #region Configuration

        /// <summary>
        ///     Ajoute une paire clé/page au service de navigation.
        /// </summary>
        /// <param name="pageKey">
        ///     La clé qui sera utilisé ensuite dans <see cref="NavigateTo(string)"/> ou <see cref="NavigateTo(string, object)"/>.
        /// </param>
        /// <param name="pageType">
        ///     Type de la page correspondante à la clé.
        /// </param>
        /// <param name="shell">
        ///     Est-ce que cette page est destinée à une navigation interne dans le shell ?
        /// </param>
        public void Configure(string pageKey, Type pageType, bool shell)
        {
            this.pagesByKey.Add(pageKey, pageType);

            if (shell)
                this.shellPages.Add(pageKey);
        }

        #endregion
    }
}
