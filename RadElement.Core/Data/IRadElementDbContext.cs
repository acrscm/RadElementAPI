using RadElement.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RadElement.Core.Data
{
    /// <summary>
    /// Repository interface for data related operations
    /// </summary>
    public interface IRadElementDbContext
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        DbSet<Code> Code { get; }

        /// <summary>
        /// Gets the code reference.
        /// </summary>
        DbSet<CodeRef> CodeRef { get; }

        /// <summary>
        /// Gets the code system.
        /// </summary>
        DbSet<CodeSystem> CodeSystem { get; }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        DbSet<Editor> Editor { get; }

        /// <summary>
        /// Gets the element.
        /// </summary>
        DbSet<Element> Element { get; }

        /// <summary>
        /// Gets the element set.
        /// </summary>
        DbSet<ElementSet> ElementSet { get; }

        /// <summary>
        /// Gets the element set reference.
        /// </summary>
        /// <value>
        DbSet<ElementSetRef> ElementSetRef { get; }

        /// <summary>
        /// Gets the element value.
        /// </summary>
        DbSet<ElementValue> ElementValue { get; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        DbSet<Image> Image { get; }

        /// <summary>
        /// Gets the image reference.
        /// </summary>
        DbSet<ImageRef> ImageRef { get; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}