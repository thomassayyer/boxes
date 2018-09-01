using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Boxes.Auxiliary.Converters
{
    /// <summary>
    ///     Convertit un booléen en valeur pour la propriété "Visibility" des contrôles XAML.
    /// </summary>
    /// <remarks>
    ///     Un booléen faux sera convertit en <see cref="Visibility.Visible"/> tandis qu'un booléen vrai induira
    ///     la valeur <see cref="Visibility.Collapsed"/>. Ce converter et donc l'inverse de 
    ///     <see cref="VisibilityConverter"/>.
    /// </remarks>
    class InverseVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Exécute la conversion du booléen en <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">
        ///     Booléen à convertir.
        /// </param>
        /// <param name="targetType">
        ///     Type de donnée attendu en fin de conversion (ici <see cref="Visibility"/>).
        /// </param>
        /// <param name="parameter">
        ///     Paramètre optionnel (aucun dans ce cas).
        /// </param>
        /// <param name="language">
        ///     Langage à utiliser dans le converter.
        /// </param>
        /// <returns>
        ///     Le booléen convertit en <see cref="Visibility"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     Exécute la conversion inverse. C'est à dire la conversion de <see cref="Visibility"/> en booléen.
        /// </summary>
        /// <param name="value">
        ///     Visibilité à convertir.
        /// </param>
        /// <param name="targetType">
        ///     Type de donnée attendu en fin de conversion (ici un booléen).
        /// </param>
        /// <param name="parameter">
        ///     Paramètre optionnel (aucun dans ce cas).
        /// </param>
        /// <param name="language">
        ///     Langage à utiliser dans le converter.
        /// </param>
        /// <returns>
        ///     <see cref="Visibility"/> convertit en booléen.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (Visibility)value == Visibility.Collapsed;
        }
    }
}
