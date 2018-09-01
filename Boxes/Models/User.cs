using Newtonsoft.Json;
using System;

namespace Boxes.Models
{
    /// <summary>
    ///     Représente un utilisateur inscrit sur Boxes.
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Identifiant unique de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        ///     Nom de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Prénom de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        ///     Numéro de tél. de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        ///     Adresse email de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        ///     Mot de passe de l'utilisateur.
        /// </summary>
        /// <remarks>
        ///     Cette propriété n'est pas incluse dans les import JSON.
        /// </remarks>
        public string Password { get; set; }

        /// <summary>
        ///     Date de naissance de l'utilisateur.
        /// </summary>
        [JsonProperty(PropertyName = "birth_date")]
        public DateTime BirthDate { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj as User)?.Id.Equals(this.Id) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
