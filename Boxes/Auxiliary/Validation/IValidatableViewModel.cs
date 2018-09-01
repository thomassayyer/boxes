namespace Boxes.Auxiliary.Validation
{
    /// <summary>
    ///     Définie l'ensemble des méthodes et propriété publiques d'un view model validable.
    /// </summary>
    interface IValidatableViewModel
    {
        /// <summary>
        ///     Effectue l'opération de validation.
        /// </summary>
        void Validate();
    }
}
