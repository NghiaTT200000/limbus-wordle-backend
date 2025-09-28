using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Limbus_wordle_backend.Util.Functions.FileUpload
{
    public class Upload
    {
        public static async Task<string> UploadToCloudinary(string url,string fileName)
        {
            try
            {
                Cloudinary cloudinary = new(Environment.EnvironmentVariables.cloudinaryUrl);
                cloudinary.Api.Secure = true;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(url),
                    PublicId=fileName,
                    UseFilename = true,
                    UniqueFilename=false,
                    Overwrite = true
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}