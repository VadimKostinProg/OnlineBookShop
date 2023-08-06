using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BookShop.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<string> AddImage(string folder, IFormFile image)
        {
            if (image == null)
                throw new ArgumentNullException("Image is missed.");

            string rootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string productPath = Path.Combine(rootPath, @$"images\{folder}");

            if (!Directory.Exists(productPath))
                Directory.CreateDirectory(productPath);

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            string imageUrl = @$"\images\{folder}\{fileName}";

            return Task.FromResult(imageUrl);
        }

        public Task<bool> DeleteImage(string imageUrl)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            if (File.Exists(rootPath + "\\" + imageUrl))
            {
                File.Delete(rootPath + "\\" + imageUrl);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetImages(string folder)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            string path = Path.Combine(rootPath, @$"images\{folder}");

            if (!Directory.Exists(path))
                return Task.FromResult(new List<string>().AsEnumerable());

            return Task.FromResult(Directory.GetFiles(path)
                .Select(file => $@"\images\{folder}\{Path.GetFileName(file)}"));
        }
    }
}
