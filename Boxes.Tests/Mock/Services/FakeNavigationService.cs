using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Linq;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service de navigation fictif dont on peut récupérer la page actuellement
    ///     affichée avec la propriété <c>CurrentPageKey</c>. Le stack de navigation
    ///     est enregistré dans une liste (champ <c>navigationStack</c>).
    /// </summary>
    class FakeNavigationService : INavigationService
    {
        #region Fields

        /// <summary>
        ///     Stack de navigation.
        /// </summary>
        /// <remarks>
        ///     A chaque fois qu'une navigation s'opère la page affichée s'inscrit
        ///     dans ce dictionnaire (en clé). Le paramètre transmis à la page
        ///     s'inscrit en valeur.
        /// </remarks>
        private readonly Dictionary<string, object> navigationStack;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise le stack de navigation.
        /// </summary>
        public FakeNavigationService()
        {
            this.navigationStack = new Dictionary<string, object>
            {
                { "RootPage", null }
            };
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        /// <summary>
        ///     Clé de la page actuellement affichée.
        /// </summary>
        public string CurrentPageKey { get; private set; }

        /// <summary>
        ///     Paramètre transmis à la page actuellement affichée.
        /// </summary>
        public object CurrentPageParameter
        {
            get
            {
                return this.navigationStack[this.CurrentPageKey];
            }
        }

        #endregion

        #region Navigation

        /// <inheritdoc />
        /// <summary>
        ///     Navigue vers la page précédente dans le stack de navigation.
        /// </summary>
        public void GoBack()
        {
            if (this.CurrentPageKey != null && this.navigationStack.ContainsKey(this.CurrentPageKey))
            {
                this.navigationStack.Remove(this.CurrentPageKey);

                this.CurrentPageKey = this.navigationStack.Keys.ElementAt(this.navigationStack.Count - 1);
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Navigue vers la page de clé <paramref name="pageKey"/>. La propriété
        ///     <c>CurrentPageKey</c> prend donc la valeur de <paramref name="pageKey"/>.
        /// </summary>
        /// <param name="pageKey">
        ///     Clé de la page sur laquelle naviguer.
        /// </param>
        public void NavigateTo(string pageKey)
        {
            this.NavigateTo(pageKey, null);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Navigue vers la page de clé <paramref name="pageKey"/> en lui passant
        ///     un paramètre. La propriété <c>CurrentPageKey</c> prend donc la valeur
        ///     de <paramref name="pageKey"/>. La propriété <c>CurrentPageParameter</c>
        ///     prend la valeur de <paramref name="parameter"/>.
        /// </summary>
        /// <param name="pageKey">
        ///     Clé de la page sur laquelle naviguer.
        /// </param>
        /// <param name="parameter">
        ///     Paramètre à passer à la page.
        /// </param>
        public void NavigateTo(string pageKey, object parameter)
        {
            this.CurrentPageKey = pageKey;

            this.navigationStack.Add(pageKey, parameter);
        }

        #endregion
    }
}
