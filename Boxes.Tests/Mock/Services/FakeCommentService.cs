using System.Collections.Generic;
using System.Threading.Tasks;
using Boxes.Models;
using Boxes.Services.Comment;
using System.Linq;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données fictives de l'entité <see cref="Post"/>.
    /// </summary>
    class FakeCommentService : ICommentService
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste des commentaires
        ///     fictifs.
        /// </summary>
        public FakeCommentService()
        {
            this.Comments = new List<Comment>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Liste de commentaires fictifs.
        /// </summary>
        public List<Comment> Comments { get; private set; }

        #endregion

        #region Create

        /// <inheritdoc />
        public Task<Comment> CreateAsync(Comment comment)
        {
            this.Comments.Add(comment);

            return Task.FromResult(this.Comments.Find(c => c.Equals(comment)));
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Comment>> GetByPostAsync(Post post)
        {
            return Task.FromResult(
                this.Comments.Where(c => c.Post.Equals(post)).ToList());
        }

        #endregion
    }
}
