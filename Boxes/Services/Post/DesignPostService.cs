using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Boxes.Services.Post
{
    /// <summary>
    ///     Service de design du model <see cref="Models.Post" />.
    /// </summary>
    class DesignPostService : IPostService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task<Models.Post> CreateAsync(Models.Post post)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Models.Post>> GetByBoxAsync(Models.Box box)
        {
            return Task.FromResult(new List<Models.Post>
            {
                new Models.Post
                {
                    Id = 1,
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec pellentesque venenatis eros, imperdiet lacinia arcu consequat sed.",
                    Author = new Models.User { FirstName = "John", LastName = "Doe" },
                    Box = box
                },
                new Models.Post
                {
                    Id = 2,
                    Content = "Integer eget ligula non ligula porttitor",
                    Author = new Models.User { FirstName = "David", LastName = "Pierce" },
                    Box = box
                }
            });
        }

        /// <inheritdoc />
        public Task<List<Models.Post>> GetByUserAsync(Models.User user)
        {
            return Task.FromResult(new List<Models.Post>
            {
                new Models.Post
                {
                    Id = 1,
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec pellentesque venenatis eros, imperdiet lacinia arcu consequat sed.",
                    Author = user,
                    Box = new Models.Box { Title = "Box 1" },
                },
                new Models.Post
                {
                    Id = 2,
                    Content = "Integer eget ligula non ligula porttitor",
                    Author = new Models.User { FirstName = "David", LastName = "Pierce" },
                    Box = new Models.Box { Title = "Box 2" },
                }
            });
        }

        #endregion
    }
}
