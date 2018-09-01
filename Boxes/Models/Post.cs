using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Boxes.Models
{
    /// <summary>
    ///     Représente un post (ou publication) publié dans une boite.
    /// </summary>
    public class Post
    {
        /// <summary>
        ///     Identifiant unique du post.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        ///     Contenu du post.
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        /// <summary>
        ///     Date de création du post.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Auteur du post.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public User Author { get; set; }

        /// <summary>
        ///     Boite associée au post.
        /// </summary>
        public Box Box { get; set; }

        /// <summary>
        ///     Commentaires saisis sur le post.
        /// </summary>
        [JsonProperty(PropertyName = "commentsCount")]
        public int CommentsCount { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj as Post)?.Id.Equals(this.Id) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
