using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Taller2.Interface;
using Taller2.Models;
namespace Taller2.Services

{

    public class CloudinaryService : ICloudinaryService

    {

        private readonly Cloudinary _cloudinary;



        public CloudinaryService(IOptions<CloudinarySettings> settings)

        {

            var account = new Account(

            settings.Value.CloudName,

            settings.Value.ApiKey,

            settings.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);

        }



        



        public async Task<string> UploadImage(string base64Image)

        {

            if (string.IsNullOrWhiteSpace(base64Image))

            {

                throw new ArgumentException("La imagen no puede estar vacía.");

            }



            try

            {

                var base64Data = base64Image;

                if (base64Image.Contains(","))

                {

                    base64Data = base64Image.Split(',')[1];

                }

                var imageBytes = Convert.FromBase64String(base64Data);

                await using var stream = new MemoryStream(imageBytes);

                var uploadParams = new ImageUploadParams

                {

                    File = new FileDescription("producto", stream),

                    Folder = "productos"

                };



                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error != null)

                {

                    throw new Exception($"Error al subir la imagen: " + $"{result.Error.Message}");

                }

                return result.SecureUrl.ToString();

            }

            catch (FormatException)

            {

                throw new ArgumentException(

                "El formato de la imagen Base64 no es válido.");

            }

        }









    }

}

