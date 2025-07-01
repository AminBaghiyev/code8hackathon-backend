using Management.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Management.BL.Utilities;

public static class FileManager
{
    public static async Task<string> SaveAsync(this IFormFile file, string folder)
    {
        string uploadPath = Path.Combine(Path.GetFullPath("wwwroot"), "uploads", folder);

        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string filename = Guid.NewGuid().ToString() + file.FileName;

        using (FileStream fs = new(Path.Combine(uploadPath, filename), FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }

        return filename;
    }

    public static bool CheckSize(this IFormFile file, int size, FileSizeType sizeType) => file.Length < size * ((int)sizeType);

    public static bool CheckType(this IFormFile file, params string[] requiredTypes) => requiredTypes.Any(type => file.ContentType.Contains(type, StringComparison.OrdinalIgnoreCase));
}
