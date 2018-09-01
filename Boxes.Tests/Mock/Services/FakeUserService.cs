using Boxes.Services.User;
using System.Threading.Tasks;
using Boxes.Models;
using System.Collections.Generic;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données fictives de l'entité <see cref="User"/>.
    /// </summary>
    class FakeUserService : IUserService
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste des utilisateurs
        ///     fictifs enregistrés en la remplissant.
        /// </summary>
        public FakeUserService()
        {
            this.Users = new List<User>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Utilisateurs fictifs enregistrés.
        /// </summary>
        public List<User> Users { get; private set; }

        #endregion

        #region Create

        /// <inheritdoc />
        public Task<User> CreateAsync(User user)
        {
            this.Users.Add(user);

            return Task.FromResult(this.Users.Find(u => u.Equals(user)));
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<User> GetByEmailPasswordAsync(string email, string password)
        {
            return Task.FromResult(
                this.Users.Find(u => (u.Email == email) && (u.Password == password)));
        }

        #endregion
    }
}
