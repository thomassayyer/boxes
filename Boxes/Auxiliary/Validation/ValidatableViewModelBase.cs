using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Boxes.Auxiliary.Validation
{
    /// <summary>
    ///     Classe de base de tous les view models validables par des contrôles de saisie.
    /// </summary>
    public abstract class ValidatableViewModelBase : ViewModelBase, IValidatableViewModel
    {
        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste des erreurs relatives
        ///     aux contrôles de saisie.
        /// </summary>
        public ValidatableViewModelBase()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        ///     Liste des erreurs relatives aux contrôles de saisie.
        /// </summary>
        protected List<string> Errors { get; set; }

        /// <summary>
        ///     Détermine si les données du view model sont valides.
        /// </summary>
        public bool IsValid
        {
            get { return this.Errors.Count <= 0; }
        }

        /// <inheritdoc />
        public abstract void Validate();
    }
}
