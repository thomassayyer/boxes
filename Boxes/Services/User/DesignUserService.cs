using System;
using System.Threading.Tasks;

namespace Boxes.Services.User
{
    /// <summary>
    ///     Service d'accès aux données de design de l'entité <see cref="Models.User" />.
    /// </summary>
    class DesignUserService : IUserService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task<Models.User> CreateAsync(Models.User user)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<Models.User> GetByEmailPasswordAsync(string email, string password)
        {
            return Task.FromResult(new Models.User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            });
        }

        #endregion
    }
}
