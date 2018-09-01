using Newtonsoft.Json;
using System.Collections.Generic;

namespace Boxes.Models
{
    /// <summary>
    ///     Représente une boite de discussion.
    /// </summary>
    public class Box
    {
        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste <c>Subscribers</c>.
        /// </summary>
        public Box()
        {
            this.Subscribers = new List<User>();
        }

        /// <summary>
        ///     Identifiant unique de la boite.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        ///     Titre de la boite.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     Description de la boite.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     Créateur de la boite.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public User Creator { get; set; }

        /// <summary>
        ///     Utilisateurs abonnés à la boite.
        /// </summary>
        [JsonProperty(PropertyName = "subscribers")]
        public List<User> Subscribers { get; set; }

        /// <summary>
        ///     Nombre de posts créés sur la boite.
        /// </summary>
        [JsonProperty(PropertyName = "postsCount")]
        public int PostsCount { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Title;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj as Box)?.Id.Equals(this.Id) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
