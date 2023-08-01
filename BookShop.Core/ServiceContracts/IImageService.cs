using Microsoft.AspNetCore.Http;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing images of application.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Method for adding new image to the passed folder.
        /// </summary>
        /// <param name="folder">Folder name to insert new image.</param>
        /// <param name="image">Image to insert.</param>
        /// <returns>Url of inserted image.</returns>
        Task<string> AddImage(string folder, IFormFile image);

        /// <summary>
        /// Method for reading all images url in passed folder.
        /// </summary>
        /// <param name="folder">Folder name to read all images.</param>
        /// <returns>Collection IEnumerable of images url.</returns>
        Task<IEnumerable<string>> GetImages(string folder);

        /// <summary>
        /// Method for deleting image by it`s url.
        /// </summary>
        /// <param name="imageUrl">URL of image to delete.</param>
        /// <returns>True - if deletion was successful, otherwise - false.</returns>
        Task<bool> DeleteImage(string imageUrl);
    }
}
