using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Boxes.Auxiliary.Converters
{
    /// <summary>
    ///     Récupère l'élément cliqué dans une <see cref="GridView"/> ou <see cref="ListView"/>.
    /// </summary>
    class ItemClickedConverter : IValueConverter
    {
        /// <summary>
        ///     Récupère l'élément cliqué en fonction des <see cref="ItemClickEventArgs"/>.
        /// </summary>
        /// <param name="value">
        ///     Arguments retounés par l'événement "ItemClick".
        /// </param>
        /// <param name="targetType">
        ///     Type de donnée attendu en fin de conversion.
        /// </param>
        /// <param name="parameter">
        ///     Paramètre optionnel (aucun dans ce cas).
        /// </param>
        /// <param name="language">
        ///     Langage à utiliser dans le converter.
        /// </param>
        /// <returns>
        ///     L'élément clické.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as ItemClickEventArgs)?.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
