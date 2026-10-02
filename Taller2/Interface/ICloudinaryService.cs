namespace Taller2.Interface
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage(string base64Image);
    }
}