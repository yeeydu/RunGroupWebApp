using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); // IFormFile handy to upload file with all properties types
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
