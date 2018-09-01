using Boxes.Services.Box;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boxes.Models;
using System.Linq;

namespace Boxes.Tests.Mock.Services
{
    /// <summary>
    ///     Service d'accès aux données fictives de l'entité <see cref="Box"/>.
    /// </summary>
    class FakeBoxService : IBoxService
    {
        #region Constructors

        /// <summary>
        ///     Constructeur non paramétré qui initialise la liste des boites
        ///     fictivement créées.
        /// </summary>
        public FakeBoxService()
        {
            this.Boxes = new List<Box>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Liste des boites fictivement créées.
        /// </summary>
        public List<Box> Boxes { get; private set; }

        #endregion

        #region Create

        /// <inheritdoc />
        public Task AttachUserAsync(Box box, User user)
        {
            Task task = Task.Run(
                () => this.Boxes.Find(b => b.Equals(box)).Subscribers.Add(user));

            task.Wait();

            return task;
        }

        /// <inheritdoc />
        public Task<Box> CreateAsync(Box box)
        {
            this.Boxes.Add(box);

            return Task.FromResult(this.Boxes.Find(b => b.Equals(box)));
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public Task<List<Box>> GetByUserAsync(User user)
        {
            return Task.FromResult(
                this.Boxes.Where(b => b.Creator.Equals(user) || b.Subscribers.Contains(user)).ToList());
        }

        /// <inheritdoc />
        public Task<List<Box>> GetSearchResultsAsync(string terms)
        {
            return Task.FromResult(
                this.Boxes.Where(b => (b.Description.Contains(terms)) || (b.Title.Contains(terms))).ToList());
        }

        /// <inheritdoc />
        public Task<List<Box>> GetTopAsync()
        {
            return Task.FromResult(
                this.Boxes.OrderBy(b => b.Subscribers.Count).Take(10).ToList());
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public Task<Box> UpdateAsync(Box box)
        {
            var index = this.Boxes.FindIndex(b => b.Equals(box));
            this.Boxes[index] = box;

            return Task.FromResult(
                this.Boxes.Find(b => b.Equals(box)));
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public Task DeleteAsync(Box box)
        {
            Task task = Task.Run(() =>
            {
                if (this.Boxes.Contains(box))
                    this.Boxes.Remove(box);
            });

            task.Wait();

            return task;
        }

        /// <inheritdoc />
        public Task DetachUserAsync(Box box, User user)
        {
            Task task = Task.Run(
                () => this.Boxes.Find(b => b.Equals(box)).Subscribers.Remove(user));

            task.Wait();

            return task;
        }

        #endregion
    }
}
