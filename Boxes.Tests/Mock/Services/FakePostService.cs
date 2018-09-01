using Boxes.Services.Post;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boxes.Models;
using System.Linq;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données fictives de l'entité <see cref="Post"/>.
    /// </summary>
    class FakePostService : IPostService
    {
        #region Properties

        /// <summary>
        ///     Posts fictivements créés.
        /// </summary>
        public List<Post> Posts { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste des posts
        ///     fictivements créés.
        /// </summary>
        public FakePostService()
        {
            this.Posts = new List<Post>();
        }

        #endregion

        #region Create

        /// <inheritdoc />
        public Task<Post> CreateAsync(Post post)
        {
            this.Posts.Add(post);

            return Task.FromResult(this.Posts.Find(p => p.Equals(post)));
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Post>> GetByBoxAsync(Box box)
        {
            return Task.FromResult(
                this.Posts.Where(p => p.Box.Equals(box)).ToList());
        }

        /// <inheritdoc />
        public Task<List<Post>> GetByUserAsync(User user)
        {
            return Task.FromResult(
                this.Posts.Where(p => p.Box.Subscribers.Contains(user) || p.Box.Creator.Equals(user)).ToList());
        }

        #endregion
    }
}
