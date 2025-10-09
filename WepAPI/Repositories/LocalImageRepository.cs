using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WepAPI.Data;
using WepAPI.Models.Domain;

namespace WepAPI.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public Image Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localFilePath, FileMode.Create);
            image.File.CopyTo(stream);
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://"+$"{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();
            return image;
        }
        public List<Image> GetAllInfoImages()
        {
            var allImages = _dbContext.Images.ToList();
            return allImages;
        }
        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var FileById = _dbContext.Images.Where(x => x.Id == Id).FirstOrDefault();
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{FileById.FileName}{FileById.FileExtension}");
                var stream = File.ReadAllBytes(path);
                var fileName = FileById.FileName + FileById.FileExtension;
                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
   

}
