using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Services.Comment
{
    /// <summary>
    ///     Service d'accès aux données de design de l'entité <see cref="Models.Comment" />.
    /// </summary>
    class DesignCommentService : ICommentService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task<Models.Comment> CreateAsync(Models.Comment comment)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Models.Comment>> GetByPostAsync(Models.Post post)
        {
            return Task.FromResult(new List<Models.Comment>
            {
                new Models.Comment
                {
                    Content = "Cool",
                    Author = new Models.User { FirstName = "John", LastName = "Doe" }
                },
                new Models.Comment
                {
                    Content = "Super !!!",
                    Author = new Models.User { FirstName = "Jane", LastName = "Doe" }
                }
            });
        }

        #endregion
    }
}
