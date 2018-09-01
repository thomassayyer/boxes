using System;
using Windows.UI.Xaml.Data;

namespace Boxes.Auxiliary.Converters
{
    /// <summary>
    ///     Réduit la taille d'une chaine de caractère et la termine par "...".
    /// </summary>
    class MaxLengthConverter : IValueConverter
    {
        /// <summary>
        ///     Réduit la taille d'une chaine de caractère et le termine par "...".
        /// </summary>
        /// <param name="value">
        ///     Chaine à convertir.
        /// </param>
        /// <param name="targetType">
        ///     Type de donnée attendu en fin de conversion (ici <see cref="string"/>).
        /// </param>
        /// <param name="parameter">
        ///     Taille maximale de la chaine.
        /// </param>
        /// <param name="language">
        ///     Langage à utiliser dans le converter.
        /// </param>
        /// <returns>
        ///     Chaine réduite.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Si la valeur est déjà plus petite que la taille maximale on annule la conversion.
            if (((string)value).Length <= (int)parameter)
                return (string)value;

            var cut = ((string)value).Substring(0, (int)parameter);

            // Retourne la valeur convertit sans couper de mots.
            return cut.Substring(0, cut.LastIndexOf(" ")) + " ...";
        }
    
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
