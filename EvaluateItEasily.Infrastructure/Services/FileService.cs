using EvaluateItEasily.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService( IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //public async Task<Result<(string,string)>> SaveFileAsync(IFormFile file, CancellationToken ct = default)
        //{
        //    if (file is null || file.Length == 0)
        //        return Result.Failure<(string, string)>(FileErrors.InvalidFile);

        //    BinaryReader binary = new(file.OpenReadStream());
        //    var bytes = binary.ReadBytes(4);

        //    var fileSequenceHex = BitConverter.ToString(bytes);
        //    if (!FileSettings.AllowedFileExtensionSignatures.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
        //        return Result.Failure<(string, string)>(FileErrors.InvalidExtension);

        //    if (file.Length > FileSettings.MaxFileSizeInBytes)
        //        return Result.Failure<(string, string)>(FileErrors.FileTooLarge);

        //    try
        //    {
        //        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //        var folderPath = Path.Combine(webRoot, FileSettings.StoragePath);

        //        if (!Directory.Exists(folderPath))
        //            Directory.CreateDirectory(folderPath);

        //        var randomFileName = Path.GetRandomFileName();
        //        var path = Path.Combine(folderPath, randomFileName);

        //        using var stream = File.Create(path);
        //        await file.CopyToAsync(stream, ct);

        //        var relativeUrl = Path.Combine(FileSettings.StoragePath, randomFileName)
        //            .Replace("\\", "/");

        //        var result = new
        //        {
        //            relativeUrl,
        //            storedFileName = randomFileName,
        //        };
        //        return Result.Success((result.relativeUrl,result.storedFileName));
        //    }
        //    catch
        //    {
        //        return Result.Failure<(string, string)>(FileErrors.SaveFailed);
        //    }
        //}

        public async Task<Result<(string, string)>> SaveFileAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                return Result.Failure<(string, string)>(FileErrors.InvalidFile);

            var bytes = new byte[4];
            using (var headerStream = file.OpenReadStream())
                await headerStream.ReadAsync(bytes, 0, 4, ct);

            var fileSequenceHex = BitConverter.ToString(bytes);
            if (!FileSettings.AllowedFileExtensionSignatures.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                return Result.Failure<(string, string)>(FileErrors.InvalidExtension);

            if (file.Length > FileSettings.MaxFileSizeInBytes)
                return Result.Failure<(string, string)>(FileErrors.FileTooLarge);

            try
            {
                var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var folderPath = Path.Combine(webRoot, FileSettings.StoragePath);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var randomFileName = Path.GetRandomFileName();
                var path = Path.Combine(folderPath, randomFileName);

                await using var stream = File.Create(path);
                await file.CopyToAsync(stream, ct);

                var relativeUrl = Path.Combine(FileSettings.StoragePath, randomFileName)
                    .Replace("\\", "/");

                return Result.Success((relativeUrl, randomFileName));
            }
            catch
            {
                return Result.Failure<(string, string)>(FileErrors.SaveFailed);
            }
        }

        public void DeleteFile(string fileUrl)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, fileUrl);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        public async Task<Result<byte[]>> GetFileAsync(string relativeUrl, CancellationToken ct = default)
        {
            try
            {
                var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fullPath = Path.Combine(webRoot, relativeUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (!File.Exists(fullPath))
                    return Result.Failure<byte[]>(FileErrors.FileNotFound);

                var bytes = await File.ReadAllBytesAsync(fullPath, ct);
                return Result.Success(bytes);
            }
            catch
            {
                return Result.Failure<byte[]>(FileErrors.SaveFailed);
            }
        }
    }
}
