using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boxes.Services.Box
{
    /// <summary>
    ///     Service d'accès aux données de design de l'entité <see cref="Models.Box" />.
    /// </summary>
    class DesignBoxService : IBoxService
    {
        #region Create

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task<Models.Box> CreateAsync(Models.Box box)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task AttachUserAsync(Models.Box box, Models.User user)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Models.Box>> GetByUserAsync(Models.User user)
        {
            return Task.FromResult(new List<Models.Box>
            {
                new Models.Box
                {
                    Id = 1,
                    Title = "Lorem",
                    Description = "Vestibulum non enim eu lectus vulputate dignissim. Curabitur nec augue at libero egestas commodo. Proin sed neque bibendum, ornare dui id, convallis leo. Nunc mauris metus, pharetra at consequat vitae, imperdiet nec metus. Vivamus ac vehicula eros.",
                    Creator = user
                },
                new Models.Box
                {
                    Id = 2,
                    Title = "Ipsum",
                    Description = "Vestibulum non enim eu lectus vulputate dignissim. Curabitur nec augue at libero egestas commodo. Proin sed neque bibendum, ornare dui id, convallis leo. Nunc mauris metus, pharetra at consequat vitae, imperdiet nec metus. Vivamus ac vehicula eros.",
                    Creator = user
                }
            });
        }

        /// <inheritdoc />
        public Task<List<Models.Box>> GetTopAsync()
        {
            return Task.FromResult(new List<Models.Box>
            {
                new Models.Box
                {
                    Id = 1,
                    Title = "Lorem ipsum",
                    Creator = new Models.User { FirstName = "John", LastName = "Doe" },
                    Description = "Vestibulum non enim eu lectus vulputate dignissim. Curabitur nec augue at libero egestas commodo. Proin sed neque bibendum, ornare dui id, convallis leo. Nunc mauris metus, pharetra at consequat vitae, imperdiet nec metus. Vivamus ac vehicula eros."
                },
                new Models.Box
                {
                    Id = 2,
                    Title = "Dolor sit amet",
                    Creator = new Models.User { FirstName = "David", LastName = "Pierce" },
                    Description = "Vestibulum non enim eu lectus vulputate dignissim. Curabitur nec augue at libero egestas commodo. Proin sed neque bibendum, ornare dui id, convallis leo. Nunc mauris metus, pharetra at consequat vitae, imperdiet nec metus. Vivamus ac vehicula eros."
                }
            });
        }

        /// <inheritdoc />
        public Task<List<Models.Box>> GetSearchResultsAsync(string terms)
        {
            return Task.FromResult(new List<Models.Box>
            {
                new Models.Box
                {
                    Id = 1,
                    Title = "Mauris",
                    Creator = new Models.User { FirstName = "Jane", LastName = "Doe" },
                    Description = "Mauris maximus massa lectus, ac ultricies lacus tempor non. Aenean nunc augue, varius non venenatis eu, pulvinar ac enim."
                },
                new Models.Box
                {
                    Id = 2,
                    Title = "Curabitur",
                    Creator = new Models.User { FirstName = "Nicole", LastName = "Mc Donald" },
                    Description = "Curabitur id lacus nulla. Donec porttitor augue neque, eu condimentum orci semper sit amet. Sed ut dui nibh. Nunc ut ex quam."
                }
            });
        }

        #endregion

        #region Update

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task<Models.Box> UpdateAsync(Models.Box box)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task DetachUserAsync(Models.Box box, Models.User user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">
        ///     Levée pour chaque appel à cette méthode en mode design.
        /// </exception>
        public Task DeleteAsync(Models.Box box)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
