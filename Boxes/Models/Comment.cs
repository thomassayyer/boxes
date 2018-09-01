using Newtonsoft.Json;
using System;

namespace Boxes.Models
{
    /// <summary>
    ///     Représente le commentaire d'un post.
    /// </summary>
    public class Comment
    {
        /// <summary>
        ///     Identifiant unique du commentaire.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        ///     Contenu du commentaire.
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        /// <summary>
        ///     Date de création du commentaire.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Auteur du commentaire.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public User Author { get; set; }

        /// <summary>
        ///     Post associé au commentaire.
        /// </summary>
        [JsonProperty(PropertyName = "post")]
        public Post Post { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj as Comment)?.Id.Equals(this.Id) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
